using Dapper;

namespace ServerGrpc.DB.Table
{
    public class ConsumeItemDB : IDataTable
    {
        public const string Table = "t_consume_item";
        public long item_no { get; set; }
        public int owner { get; set; }
        public int id { get; set; }
        public int amount { get; set; }
        public DateTime update_time { get; set; }
        public static readonly TimeSpan expire = TimeSpan.FromMinutes(5);
        public static ConsumeItemDB Create(int charNo, int itemId, DateTime now, int count = 0)
        {
            return new ConsumeItemDB
            {
                owner = charNo,
                id = itemId,
                amount = count,
                update_time = now,
            };
        }

        #region Query
        public static (string, DynamicParameters) Insert(ConsumeItemDB db)
        {
            var query = @$"insert into {Table} ({nameof(owner)}, {nameof(id)}, {nameof(amount)}, {nameof(update_time)})
                            values (@{nameof(owner)}, @{nameof(id)}, @{nameof(amount)}, @{nameof(update_time)})";

            var param = new DynamicParameters();
            param.Add(nameof(owner), db.owner);
            param.Add(nameof(id), db.id);
            param.Add(nameof(amount), db.amount);
            param.Add(nameof(update_time), db.update_time);
            return (query, param);
        }

        public static (string, DynamicParameters) Select(int charNo)
        {
            var query = $"select * from {Table} where {nameof(owner)}=@{nameof(owner)}";
            var param = new DynamicParameters();
            param.Add(nameof(owner), charNo);
            return (query, param);
        }

        public static (string, DynamicParameters) Update(ConsumeItemDB db)
        {
            var query = $"update {Table} set {nameof(amount)}=@{nameof(amount)}, {nameof(update_time)}=@{nameof(update_time)} where {nameof(item_no)}=@{nameof(item_no)}";
            var param = new DynamicParameters();
            param.Add(nameof(amount), db.amount);
            param.Add(nameof(update_time), db.update_time);
            param.Add(nameof(item_no), db.item_no);
            return (query, param);
        }

        public static (string, DynamicParameters) Delete(long itemIndex)
        {
            var query = $"delete from {Table} where {nameof(item_no)}=@{nameof(item_no)}";
            var param = new DynamicParameters();
            param.Add(nameof(item_no), itemIndex);
            return (query, param);
        }
        #endregion
    }
}
