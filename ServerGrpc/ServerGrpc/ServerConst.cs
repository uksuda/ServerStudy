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

        public const string CONTEXT_ERROR = "Session_Error";

        //
        public const int NAME_LENGTH = 20;
        public const int CHARACTER_COUNT_MAX = 3;
        public const int START_LEVEL = 1;
        public const int START_EXP = 0;
    }
}
