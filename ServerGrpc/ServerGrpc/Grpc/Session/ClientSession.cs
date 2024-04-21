namespace ServerGrpc.Grpc.Session
{
    public class ClientSession
    {
        public int MberNo { get; private set; }
        public string Xtid { get; private set; }
        public string Xtid_Stream { get; private set; }

        public ClientSession(int mberNo, string xtid)
        {
            MberNo = mberNo;
            Xtid = xtid;
            Xtid_Stream = string.Empty;
        }

        public void SetXtidStream(string xtid)
        {
            Xtid_Stream = xtid;
        }
    }
}
