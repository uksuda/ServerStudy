using ServerGrpc.Common;
using StackExchange.Redis;
using System.Net;

namespace ServerGrpc.DB
{
    public abstract class RedisContext
    {
        private const int _SyncTimeOut = 1000;

        private readonly int _databaseNumber;
        private readonly ConnectionMultiplexer _cache;
        private readonly EndPoint _endPoint;

        #region Cache
        public IDatabase Cache()
        {
            if (_cache == null || _cache.IsConnected == false)
            {
                throw new RedisException("invalid cache state");
            }
            return _cache.GetDatabase(_databaseNumber);
        }

        public IServer CacheServer()
        {
            if (_cache == null || _cache.IsConnected == false)
            {
                throw new RedisException("invalid cache state");
            }
            return _cache.GetServer(_endPoint);
        }

        public ISubscriber GetSubscriber()
        {
            if (_cache == null || _cache.IsConnected == false)
            {
                throw new RedisException("invalid cache state");
            }
            return _cache.GetSubscriber();
        }
        #endregion

        public RedisContext(AppSettings appsettings, CacheType type)
        {
            if (appsettings.Caches == null)
            {
                throw new ArgumentNullException("appsettings cache is empty");
            }

            if (appsettings.Caches.TryGetValue(type, out var cache) == false)
            {
                throw new ArgumentNullException($"invalid cache type : {type}");
            }

            if (IPAddress.TryParse(cache.Host, out var ipAddr) == false)
            {
                var address = Dns.GetHostAddresses(cache.Host);
                ipAddr = address[0];
            }

            //if (ipAddr == null)
            //{
            //    throw new ArgumentNullException($"invalid cache endpoint : {cache.Host}");
            //}

            var connectStr = ipAddr.ToString() + $":{cache.Port}";
            _cache = CreateConnect(connectStr, cache.Password);
            _databaseNumber = cache.Database;
            _endPoint = new IPEndPoint(ipAddr, cache.Port);
        }

        private ConnectionMultiplexer CreateConnect(string connectionString, string password)
        {
            var options = new ConfigurationOptions
            {
                EndPoints = { connectionString },
                AbortOnConnectFail = false,
                SocketManager = new SocketManager(),
                SyncTimeout = _SyncTimeOut,
            };

            if (string.IsNullOrEmpty(password) == false)
            {
                options.Password = password;
            }

            var multiplexer = ConnectionMultiplexer.Connect(options);
            return multiplexer;
        }
    }

    public class AccountRedisContext : RedisContext
    {
        public AccountRedisContext(AppSettings appsettings) : base(appsettings, CacheType.Account)
        {
        }
    }

    public class GameRedisContext : RedisContext
    {
        public GameRedisContext(AppSettings appsettings) : base(appsettings, CacheType.Game)
        {
        }
    }
}
