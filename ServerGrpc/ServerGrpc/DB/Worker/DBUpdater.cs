using Dapper;
using MySqlConnector;
using System.Threading.Channels;

namespace ServerGrpc.DB.Worker
{
    public class DBUpdateInfo
    {
        public DataBaseType DBType { get; private set; }
        public string Query { get; private set; }
        public DynamicParameters Params { get; private set; }
        public string Table { get; private set; }
        public DateTime Regist { get; private set; }

        public DBUpdateInfo(
            DataBaseType type,
            string query,
            DynamicParameters param,
            string table)
        {
            DBType = type;
            Query = query;
            Params = param;
            Table = table;

            Regist = DateTime.UtcNow;
        }
    }

    public class DBUpdater
    {
        private const int _delayTime = 60 * 1000;   // 60s

        private readonly CancellationTokenSource _tokenSource;
        private readonly ILogger<DBUpdater> _logger;

        private readonly AccountDBContext _accountDbCtx;
        private readonly GameDBContext _gameDbCtx;

        private readonly ChannelWriter<DBUpdateInfo> _channelWriter;
        private readonly ChannelReader<DBUpdateInfo> _channelReader;

        public DBUpdater(
            ILogger<DBUpdater> logger,
            CancellationTokenSource tokenSource,
            AccountDBContext accountDbCtx,
            GameDBContext gameDbCtx)
        {
            _tokenSource = CancellationTokenSource.CreateLinkedTokenSource(tokenSource.Token);

            _logger = logger;

            _accountDbCtx = accountDbCtx;
            _gameDbCtx = gameDbCtx;

            var channel = Channel.CreateUnbounded<DBUpdateInfo>();
            _channelWriter = channel;
            _channelReader = channel;

            Update();
        }

        public async ValueTask Write(DBUpdateInfo updateInfo)
        {
            await _channelWriter.WriteAsync(updateInfo);
        }

        public async ValueTask Stop()
        {
            _tokenSource.Cancel();
            //while (_channelReader.TryRead(out DBSelectInfo s))
            //{
            //    if (s == null)
            //    {
            //        continue;
            //    }
            //    var r = await Select(s);
            //    s.Release(r);
            //}

            var remains = _channelReader.ReadAllAsync().ToBlockingEnumerable();
            await Execute(remains);
        }

        private void Update()
        {
            List<DBUpdateInfo> list = new List<DBUpdateInfo>();

            var task = Task.Run(async () =>
            {
                while (_tokenSource.IsCancellationRequested == false)
                {
                    list.Clear();
                    while (_channelReader.TryRead(out var updateInfo))
                    {
                        if (updateInfo == null)
                        {
                            continue;
                        }

                        list.Add(updateInfo);
                    }

                    await Execute(list);
                    await Task.Delay(_delayTime);
                }

            }, _tokenSource.Token);
        }

        private async ValueTask Execute(IEnumerable<DBUpdateInfo> infos)
        {
            foreach (DataBaseType e in Enum.GetValues(typeof(DataBaseType)))
            {
                if (e == DataBaseType.None)
                {
                    continue;
                }

                var list = MergeAndSort(e, infos);
                if (list == null || list.Any() == false)
                {
                    continue;
                }
                await Execute(e, list);
            }
        }

        private IEnumerable<DBUpdateInfo> MergeAndSort(DataBaseType type, IEnumerable<DBUpdateInfo> infos)
        {
            if (infos == null || infos.Any() == false)
            {
                return null;
            }

            var temp = infos.Where(x => x.DBType == type).ToLookup(x => x.Table)
                .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y.Regist));

            var result = new List<DBUpdateInfo>();
            foreach (var info in temp)
            {
                result.Add(info.Value.First());
            }
            return result;
        }

        private MySqlConnection GetDBContext(DataBaseType type)
        {
            switch (type)
            {
                case DataBaseType.Account:
                    return _accountDbCtx.GetDbContext();
                case DataBaseType.Game:
                    return _gameDbCtx.GetDbContext();
                default:
                    return _gameDbCtx.GetDbContext();
            }
        }

        private async ValueTask Execute(DataBaseType type, IEnumerable<DBUpdateInfo> infos)
        {
            using var dbCtx = GetDBContext(type);
            using var tran = await dbCtx.BeginTransactionAsync();

            try
            {
                foreach (var info in infos)
                {
                    await dbCtx.ExecuteAsync(info.Query, info.Params, transaction: tran);
                }
                await tran.CommitAsync();
            }
            catch (Exception e)
            {
                await tran.RollbackAsync();
                _logger.LogError(e, $"execute fail {infos.Select(x => x.Query)}");
            }
        }
    }
}
