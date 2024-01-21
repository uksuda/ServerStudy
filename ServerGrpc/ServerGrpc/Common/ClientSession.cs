namespace ServerGrpc.Common
{
    public class ClientSession
    {
        private readonly int _mberNo;
        private readonly string _xtid;
        private readonly string _id;

        public int MBER_NO => _mberNo;
        public string XTID => _xtid;
        public string ID => _id;

        public ClientSession(int mberNo, string xtid, string id)
        {
            _mberNo = mberNo;
            _xtid = xtid;
            _id = id;
        }
    }
}
