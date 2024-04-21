using Dapper;

namespace ServerGrpc.DB.Table
{
    public class CharacterDB : IDataTable
    {
        public const string Table = "t_character";
        public int mber_no {  get; set; }
        public byte character_no { get; set; }
        public string job { get; set; }
        public string nickname { get; set; }
        public int lv { get; set; }
        public long exp { get; set; }
        public DateTime create_time { get; set; }
        public DateTime update_time { get; set; }

        public static TimeSpan Expire => TimeSpan.FromMinutes(1);

        public static CharacterDB Create(int mberNo, byte charNo, string jobStr, string name, int level, long expVal, DateTime createTime, DateTime updateTime)
        {
            return new CharacterDB
            {
                mber_no = mberNo,
                character_no = charNo,
                job = jobStr,
                nickname = name,
                lv = level,
                exp = expVal,
                create_time = createTime,
                update_time = updateTime
            };
        }

        #region Query
        public static (string, DynamicParameters) Select(int mberNo)
        {
            var query = $@"select * from {Table} where {nameof(mber_no)}=@{nameof(mber_no)}";
            var param = new DynamicParameters();
            param.Add(nameof(mber_no), mberNo);
            return (query, param);
        }

        public static (string, DynamicParameters) Update(CharacterDB db)
        {
            var query = $@"update {Table} set {nameof(lv)}=@{nameof(lv)}, {nameof(exp)}=@{nameof(exp)}, {nameof(update_time)}=@{nameof(update_time)} where {nameof(mber_no)}=@{nameof(mber_no)}";
            var param = new DynamicParameters();
            param.Add(nameof(mber_no), db.mber_no);
            param.Add(nameof(lv), db.lv);
            param.Add(nameof(exp), db.exp);
            param.Add(nameof(update_time), db.update_time);
            return (query, param);
        }

        public static (string, DynamicParameters) Insert(CharacterDB db)
        {
            var query = $@"insert into {Table} ({nameof(mber_no)}, {nameof(character_no)}, {nameof(job)}, {nameof(nickname)}, {nameof(lv)}, {nameof(exp)}, {nameof(create_time)}, {nameof(update_time)})
                           values (@{nameof(mber_no)}, @{nameof(character_no)}, @{nameof(job)}, @{nameof(nickname)}, @{nameof(lv)}, @{nameof(exp)}, @{nameof(create_time)}, @{nameof(update_time)})";

            var param = new DynamicParameters();
            param.Add(nameof(mber_no), db.mber_no);
            param.Add(nameof(character_no), db.character_no);
            param.Add(nameof(job), db.job);
            param.Add(nameof(nickname), db.nickname);
            param.Add(nameof(lv), db.lv);
            param.Add(nameof(exp), db.exp);
            param.Add(nameof(create_time), db.create_time);
            param.Add(nameof(update_time), db.update_time);

            return (query, param);
        }
        #endregion
    }
}
