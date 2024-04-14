using Dapper;
using MySqlConnector;
using ServerGrpc.DB.Table;
using System.Threading.Channels;

namespace ServerGrpc.DB.Worker
{
    public class DBSelectInfo
    {
        private readonly TaskCompletionSource<IEnumerable<IDataTable>> _completionSource;

        public DataBaseType DBType { get; private set; }
        public string Query { get; private set; }
        public DynamicParameters Params { get; private set; }

        public DBSelectInfo(DataBaseType type, string query, DynamicParameters param)
        {
            DBType = type;
            Query = query;
            Params = param;

            _completionSource = new TaskCompletionSource<IEnumerable<IDataTable>>();
        }

        public async Task<IEnumerable<IDataTable>> CallAsync()
        {
            return await _completionSource.Task;
        }

        public void Release(IEnumerable<IDataTable> result)
        {
            _completionSource.SetResult(result);
        }
    }

    public class DBSelector
    {
        private readonly CancellationTokenSource _tokenSource;

        private readonly ILogger<DBSelector> _logger;
        private readonly AccountDBContext _accountDbCtx;
        private readonly GameDBContext _gameDbCtx;

        private readonly ChannelWriter<DBSelectInfo> _channelWriter;
        private readonly ChannelReader<DBSelectInfo> _channelReader;

        public DBSelector(
            ILogger<DBSelector> logger, 
            CancellationTokenSource tokenSource,
            AccountDBContext accountDb,
            GameDBContext gameDb)
        {
            _tokenSource = CancellationTokenSource.CreateLinkedTokenSource(tokenSource.Token);

            _logger = logger;
            _accountDbCtx = accountDb;
            _gameDbCtx = gameDb;

            var channel = Channel.CreateUnbounded<DBSelectInfo>();
            _channelWriter = channel;
            _channelReader = channel;

            Update();
        }

        public async ValueTask Write(DBSelectInfo selectInfo)
        {
            await _channelWriter.WriteAsync(selectInfo);
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
            foreach (var s in remains)
            {
                if (s == null)
                {
                    continue;
                }
                var r = await Select(s);
                s.Release(r);
            }
        }

        private void Update()
        {
            var task = Task.Run(async () =>
            {
                await foreach (var s in _channelReader.ReadAllAsync(_tokenSource.Token))
                {
                    if (s == null)
                    {
                        continue;
                    }
                    var r = await Select(s);
                    s.Release(r);

                    await Task.Delay(1);
                }
            }, _tokenSource.Token);
        }

        private MySqlConnection GetDBContext(DBSelectInfo info)
        {
            switch (info.DBType)
            {
                case DataBaseType.Account:
                    return _accountDbCtx.GetDbContext();
                case DataBaseType.Game:
                    return _gameDbCtx.GetDbContext();
                default:
                    return _gameDbCtx.GetDbContext();
            }
        }

        private async Task<IEnumerable<IDataTable>> Select(DBSelectInfo info)
        {
            try
            {
                using var dbCtx = GetDBContext(info);
                var result = await dbCtx.QueryAsync<IDataTable>(info.Query, info.Params);
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Select fail {info.Query}");
                return null;
            }
        }
    }
}
