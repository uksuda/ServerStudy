namespace ServerGrpc.Common
{
    public class ClientSession
    {
        private readonly string _xtid;
        private readonly string _id;
        private readonly string _password;

        public string XTID => _xtid;
        public string ID => _id;
        public string PASS => _password;

        public ClientSession(string xtid, string id, string password)
        {
            _xtid = xtid;
            _id = id;
            _password = password;
        }
    }
}
