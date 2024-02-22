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
            //TODO : pool값 수정해야함
            return $"server={Host};port={Port};user={User};password={Password};database={Database};Min Pool Size={PoolCount};Max Pool Size={PoolCount + 20};Allow User Variables=True";
        }
    }

    public class RedisSection
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }

        public (string, string) GetConnectionAndPassword()
        {
            if (string.IsNullOrEmpty(Password))
            {
                return ($"{Host}:{Port}", string.Empty);
            }
            else
            {
                return ($"{Host}:{Port}", Password);
            }
        }
    }
    #endregion

    public class AppSettings
    {
        public Dictionary<DataBaseType, DatabaseSection> Database { get; set; }
        public RedisSection Caches { get; set; }
    }
}
