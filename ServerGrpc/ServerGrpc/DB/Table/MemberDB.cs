using Dapper;

namespace ServerGrpc.DB.Table
{
    public class MemberDB : IDataTable
    {
        public const string Table = "t_member";
        public int mber_no { get; set; }
        public string id { get; set; }
        public string password { get; set; }
        public DateTime create { get; set; }
        public DateTime last_login { get; set; }

        public static TimeSpan Expire => TimeSpan.FromMinutes(1);

        public static MemberDB Create(int mberNo, string idStr, string passStr, DateTime createTime, DateTime loginTime)
        {
            
            return new MemberDB
            {
                mber_no = mberNo,
                id = idStr,
                password = passStr,
                create = createTime,
                last_login = loginTime
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

        public static (string, DynamicParameters) Select(string id)
        {
            var query = $@"select * from {Table} where {nameof(id)}=@{nameof(id)}";
            var param = new DynamicParameters();
            param.Add(nameof(id), id);
            return (query, param);
        }

        public static (string, DynamicParameters) Update(int mberNo, DateTime now)
        {
            var query = $@"update {Table} set {nameof(last_login)}=@{nameof(last_login)} where {nameof(mber_no)}=@{nameof(mber_no)}";
            var param = new DynamicParameters();
            param.Add(nameof(mber_no), mberNo);
            param.Add(nameof(last_login), now);
            return (query, param);
        }

        public static (string, DynamicParameters) Insert(MemberDB db)
        {
            var query = $@"insert into {Table} ({nameof(id)}, {nameof(password)}, {nameof(create)}, {nameof(last_login)})
                           values (@{nameof(id)}, @{nameof(password)}, @{nameof(create)}, @{nameof(last_login)})";

            var param = new DynamicParameters();
            param.Add(nameof(id), db.id);
            param.Add(nameof(password), db.password);
            param.Add(nameof(create), db.create);
            param.Add(nameof(last_login), db.last_login);

            return (query, param);
        }
        #endregion
    }
}
