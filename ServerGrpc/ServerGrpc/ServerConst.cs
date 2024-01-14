namespace ServerGrpc
{
    public static class ServerConst
    {
        public const int GRPC_MAX_SEND_SIZE = 1024 * 1024 * 4;
        public const int GRPC_MAX_RECV_SIZE = 1024 * 1024 * 4;

        public const string GRPC_COMPRESS_ALGORITHM = "gzip";
    }
}
