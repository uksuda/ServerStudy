namespace ServerGrpc.Common
{
    public class ClientSession
    {
        private readonly string _xtid;
        private readonly string _id;

        public string XTID => _xtid;
        public string ID => _id;

        public ClientSession(string xtid, string id)
        {
            _xtid = xtid;
            _id = id;
        }
    }
}
