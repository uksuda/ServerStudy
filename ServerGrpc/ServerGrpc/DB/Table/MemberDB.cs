namespace ServerGrpc.DB.Table
{
    public class MemberDB
    {
        public const string Table = "t_member";
        public int mber_no { get; set; }
        public string id { get; set; }
        public string password { get; set; }
        public DateTime create { get; set; }
    }
}
