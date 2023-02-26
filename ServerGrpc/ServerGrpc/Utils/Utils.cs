namespace ServerGrpc.Utils
{
    public static class CommonManager
    {
        public static string GetRandomKey()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
