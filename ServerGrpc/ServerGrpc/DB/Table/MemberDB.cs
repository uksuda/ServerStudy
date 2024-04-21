using Dapper;

namespace ServerGrpc.DB.Table
{
    public class MemberDB : IDataTable
    {
        public const string Table = "t_member";
        public int mber_no { get; set; }
        public string member_id { get; set; }
        public string password { get; set; }
        public DateTime create_time { get; set; }
        public DateTime last_login_time { get; set; }

        public static TimeSpan Expire => TimeSpan.FromMinutes(1);

        public static MemberDB Create(int mberNo, string memberId, string pass, DateTime createTime, DateTime loginTime)
        {
            
            return new MemberDB
            {
                mber_no = mberNo,
                member_id = memberId,
                password = pass,
                create_time = createTime,
                last_login_time = loginTime
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
            var query = $@"select * from {Table} where {nameof(member_id)}=@{nameof(member_id)}";
            var param = new DynamicParameters();
            param.Add(nameof(member_id), id);
            return (query, param);
        }

        public static (string, DynamicParameters) Update(int mberNo, DateTime now)
        {
            var query = $@"update {Table} set {nameof(last_login_time)}=@{nameof(last_login_time)} where {nameof(mber_no)}=@{nameof(mber_no)}";
            var param = new DynamicParameters();
            param.Add(nameof(mber_no), mberNo);
            param.Add(nameof(last_login_time), now);
            return (query, param);
        }

        public static (string, DynamicParameters) Insert(MemberDB db)
        {
            var query = $@"insert into {Table} ({nameof(member_id)}, {nameof(password)}, {nameof(create_time)}, {nameof(last_login_time)})
                           values (@{nameof(member_id)}, @{nameof(password)}, @{nameof(create_time)}, @{nameof(last_login_time)})";

            var param = new DynamicParameters();
            param.Add(nameof(member_id), db.member_id);
            param.Add(nameof(password), db.password);
            param.Add(nameof(create_time), db.create_time);
            param.Add(nameof(last_login_time), db.last_login_time);

            return (query, param);
        }
        #endregion
    }
}
