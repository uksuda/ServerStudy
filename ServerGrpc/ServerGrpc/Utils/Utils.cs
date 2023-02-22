namespace ServerGrpc.Utils
{
    public class CommonManager
    {
        public string GetRandomKey()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
