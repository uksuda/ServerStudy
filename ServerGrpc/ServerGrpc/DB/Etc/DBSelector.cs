using Dapper;
using ServerGrpc.DB.Table;
using System.Threading.Channels;

namespace ServerGrpc.DB.Etc
{
    public class DBSelectInfo
    {
        private readonly TaskCompletionSource<IEnumerable<IDataTable>> _completionSource;

        public string Query { get; private set; }
        public DynamicParameters Params { get; private set; }

        public DBSelectInfo(string query, DynamicParameters param)
        {
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
        private readonly AccountRedisContext _accountRedis;
        private readonly GameDBContext _gameDbCtx;
        private readonly GameRedisContext _gameRedis;

        private readonly ChannelWriter<DBSelectInfo> _channelWriter;
        private readonly ChannelReader<DBSelectInfo> _channelReader;

        public DBSelector(
            ILogger<DBSelector> logger, 
            AccountDBContext accountDb,
            AccountRedisContext accountRedis,
            GameDBContext gameDb,
            GameRedisContext gameRedis)
        {
            _tokenSource = new CancellationTokenSource();

            _logger = logger;
            _accountDbCtx = accountDb;
            _accountRedis = accountRedis;
            _gameDbCtx = gameDb;
            _gameRedis = gameRedis;

            var channel = Channel.CreateUnbounded<DBSelectInfo>();
            _channelWriter = channel;
            _channelReader = channel;
        }

        public async ValueTask WriteSelectReq(DBSelectInfo selectInfo)
        {
            await _channelWriter.WriteAsync(selectInfo);
        }

        public async ValueTask Stop()
        {
            _tokenSource.Cancel();
            while (_channelReader.TryRead(out DBSelectInfo s))
            {
                //
            }

            //var remains = _channelReader.ReadAllAsync().ToBlockingEnumerable();
            //foreach (var s in remains)
            //{
            //    //
            //}
        }

        private void UpdateReq()
        {
            Task.Run(async () =>
            {

            }, _tokenSource.Token);
        }
    }
}
