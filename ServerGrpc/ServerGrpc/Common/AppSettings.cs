namespace ServerGrpc.Common
{
    #region DataBase & Caches
    public class DatabaseSection
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }
        public int PoolCount { get; set; }

        public string GetConnectionString()
        {
            return $"server={Host};port={Port};user={User};password={Password};database={Database};Min Pool Size={PoolCount};Max Pool Size={PoolCount + 20};Allow User Variables=True";
        }
    }

    public class RedisSection
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }
        public int Database { get; set; }
    }
    #endregion

    #region Jwt
    public class JwtSection
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
    #endregion

    public class AppSettings
    {
        public JwtSection Jwt { get; set; }
        public Dictionary<DataBaseType, DatabaseSection> Database { get; set; }
        public Dictionary<CacheType, RedisSection> Caches { get; set; }
    }
}
