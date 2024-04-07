namespace ServerGrpc.DB.Table
{
    public class CharacterDB
    {
        public const string Table = "";
        public int mber_no {  get; set; }
        public byte character_no { get; set; }
        public string job { get; set; }
        public string nickname { get; set; }
        public DateTime create { get; set; }
        public DateTime update { get; set; }

    }
}
