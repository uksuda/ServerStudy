using System.Threading.Channels;

namespace ServerGrpc.DB.Etc
{
    public class DBUpdateInfo
    {

    }

    public class DBUpdater
    {
        private readonly CancellationTokenSource _tokenSource;
        private readonly ILogger<DBUpdater> _logger;

        private readonly AccountDBContext _accountDbCtx;
        private readonly AccountRedisContext _accountRedis;
        private readonly GameDBContext _gameDbCtx;
        private readonly GameRedisContext _gameRedis;

        private readonly ChannelWriter<DBUpdateInfo> _channelWriter;
        private readonly ChannelReader<DBUpdateInfo> _channelReader;

        public DBUpdater(ILogger<DBUpdater> logger)
        {
            _logger = logger;
        }
    }
}
