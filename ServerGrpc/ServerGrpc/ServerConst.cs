namespace ServerGrpc
{
    public enum DataBaseRWType
    {
        None = 0,
        Read,
        ReadWrite,
    }

    public enum DataBaseType
    {
        None = 0,
        Account,
        Game,
        Log
    }

    public enum CacheType
    {
        None = 0,
        Account,
        Game,
    }

    public static class ServerConst
    {
        public const int GRPC_MAX_SEND_SIZE = 1024 * 1024 * 4;
        public const int GRPC_MAX_RECV_SIZE = 1024 * 1024 * 4;

        public const string GRPC_COMPRESS_ALGORITHM = "gzip";
    }
}
