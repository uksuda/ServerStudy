using Dapper;
using Game.Types;

namespace ServerGrpc.DB.Table
{
    public class EquipItemDB : IDataTable
    {
        public const string Table = "t_equip_item";
        public long item_no { get; set; }
        public int owner { get; set; }
        public int id { get; set; }
        public byte grade { get; set; }
        public int att { get; set; }
        public int def { set; get; }
        public byte bonus_status_type_1 { get; set; }
        public int bonus_status_value_1 { get; set; }
        public byte bonus_status_type_2 { get; set; }
        public int bonus_status_value_2 { get; set; }
        public byte bonus_status_type_3 { get; set; }
        public int bonus_status_value_3 { get; set; }

        public byte effect_type_1 { get; set; }
        public int effect_value_1 { get; set; }
        public byte effect_type_2 { get; set; }
        public int effect_value_2 { get; set; }
        public byte effect_type_3 { get; set; }
        public int effect_value_3 { get; set; }
        public byte effect_type_4 { get; set; }
        public int effect_value_4 { get; set; }
        public byte equipped { get; set; }
        public DateTime update_time { get; set; }
        public static readonly TimeSpan expire = TimeSpan.FromMinutes(5);
        public static EquipItemDB Create(int charNo, int itemId, GradeType grade, DateTime updateTime)
        {
            return new EquipItemDB
            {
                owner = charNo,
                id = itemId,
                grade = (byte)grade,
                update_time = updateTime,
            };
        }

        #region Query
        public static (string, DynamicParameters) Insert(EquipItemDB db)
        {
            var query = @$"insert into {Table} ({nameof(owner)}, {nameof(id)}, {nameof(grade)}, {nameof(att)}, {nameof(def)}, 
                        {nameof(bonus_status_type_1)}, {nameof(bonus_status_value_1)}, {nameof(bonus_status_type_2)}, {nameof(bonus_status_value_2)}, {nameof(bonus_status_type_3)}, {nameof(bonus_status_value_3)},
                        {nameof(effect_type_1)}, {nameof(effect_value_1)}, {nameof(effect_type_2)}, {nameof(effect_value_2)}, {nameof(effect_type_3)}, {nameof(effect_value_3)}, {nameof(effect_type_4)}, {nameof(effect_value_4)},
                        {nameof(equipped)}, {nameof(update_time)})
                        values (@{nameof(owner)}, @{nameof(id)}, @{nameof(grade)}, @{nameof(att)}, @{nameof(def)},
                        @{nameof(bonus_status_type_1)}, @{nameof(bonus_status_value_1)}, @{nameof(bonus_status_type_2)}, @{nameof(bonus_status_value_2)}, @{nameof(bonus_status_type_3)}, @{nameof(bonus_status_value_3)},
                        @{nameof(effect_type_1)}, @{nameof(effect_value_1)}, @{nameof(effect_type_2)}, @{nameof(effect_value_2)}, @{nameof(effect_type_3)}, @{nameof(effect_value_3)}, @{nameof(effect_type_4)}, @{nameof(effect_value_4)},
                        @{nameof(equipped)}, @{nameof(update_time)})";
            var param = new DynamicParameters();
            param.Add(nameof(owner), db.owner);
            param.Add(nameof(id), db.id);
            param.Add(nameof(grade), db.grade);
            param.Add(nameof(att), db.att);
            param.Add(nameof(def), db.def);
            param.Add(nameof(bonus_status_type_1), db.bonus_status_type_1);
            param.Add(nameof(bonus_status_value_1), db.bonus_status_value_1);
            param.Add(nameof(bonus_status_type_2), db.bonus_status_type_2);
            param.Add(nameof(bonus_status_value_2), db.bonus_status_value_2);
            param.Add(nameof(bonus_status_type_3), db.bonus_status_type_3);
            param.Add(nameof(bonus_status_value_3), db.bonus_status_value_3);
            param.Add(nameof(effect_type_1), db.effect_type_1);
            param.Add(nameof(effect_value_1), db.effect_value_1);
            param.Add(nameof(effect_type_2), db.effect_type_2);
            param.Add(nameof(effect_value_2), db.effect_value_2);
            param.Add(nameof(effect_type_3), db.effect_type_3);
            param.Add(nameof(effect_value_3), db.effect_value_3);
            param.Add(nameof(effect_type_4), db.effect_type_4);
            param.Add(nameof(effect_value_4), db.effect_value_4);
            param.Add(nameof(equipped), db.equipped);
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

        public static (string, DynamicParameters) Update(EquipItemDB db)
        {
            var query = @$"update {Table} set {nameof(owner)}=@{nameof(owner)}, {nameof(grade)}=@{nameof(grade)}, {nameof(att)}=@{nameof(att)}, {nameof(def)}=@{nameof(def)},
                        {nameof(bonus_status_type_1)}=@{nameof(bonus_status_type_1)}, {nameof(bonus_status_value_1)}=@{nameof(bonus_status_value_1)}, 
                        {nameof(bonus_status_type_2)}=@{nameof(bonus_status_type_2)}, {nameof(bonus_status_value_2)}=@{nameof(bonus_status_value_2)}, 
                        {nameof(bonus_status_type_3)}=@{nameof(bonus_status_type_3)}, {nameof(bonus_status_value_3)}=@{nameof(bonus_status_value_3)}, 
                        {nameof(effect_type_1)}=@{nameof(effect_type_1)}, {nameof(effect_value_1)}=@{nameof(effect_value_1)}, 
                        {nameof(effect_type_2)}=@{nameof(effect_type_2)}, {nameof(effect_value_2)}=@{nameof(effect_value_2)}, 
                        {nameof(effect_type_3)}=@{nameof(effect_type_3)}, {nameof(effect_value_3)}=@{nameof(effect_value_3)}, 
                        {nameof(effect_type_4)}=@{nameof(effect_type_4)}, {nameof(effect_value_4)}=@{nameof(effect_value_4)}, 
                        {nameof(equipped)}=@{nameof(equipped)}, {nameof(update_time)}=@{nameof(update_time)} 
                        where {nameof(item_no)}=@{nameof(item_no)}";
            var param = new DynamicParameters();
            param.Add(nameof(owner), db.owner);
            param.Add(nameof(grade), db.grade);
            param.Add(nameof(att), db.att);
            param.Add(nameof(def), db.def);
            param.Add(nameof(bonus_status_type_1), db.bonus_status_type_1);
            param.Add(nameof(bonus_status_value_1), db.bonus_status_value_1);
            param.Add(nameof(bonus_status_type_2), db.bonus_status_type_2);
            param.Add(nameof(bonus_status_value_2), db.bonus_status_value_2);
            param.Add(nameof(bonus_status_type_3), db.bonus_status_type_3);
            param.Add(nameof(bonus_status_value_3), db.bonus_status_value_3);
            param.Add(nameof(effect_type_1), db.effect_type_1);
            param.Add(nameof(effect_value_1), db.effect_value_1);
            param.Add(nameof(effect_type_2), db.effect_type_2);
            param.Add(nameof(effect_value_2), db.effect_value_2);
            param.Add(nameof(effect_type_3), db.effect_type_3);
            param.Add(nameof(effect_value_3), db.effect_value_3);
            param.Add(nameof(effect_type_4), db.effect_type_4);
            param.Add(nameof(effect_value_4), db.effect_value_4);
            param.Add(nameof(equipped), db.equipped);
            param.Add(nameof(update_time), db.update_time);
            param.Add(nameof(item_no), db.item_no);
            return (query, param);
        }

        public static (string, DynamicParameters) UpdateEquipped(EquipItemDB db)
        {
            var query = $@"update {Table} set {nameof(equipped)}=@{nameof(equipped)} where {nameof(item_no)}=@{nameof(item_no)}";
            var param = new DynamicParameters();
            param.Add(nameof(equipped), db.equipped);
            param.Add(nameof(item_no), db.item_no);
            return (query, param);
        }

        public static (string, DynamicParameters) UpdateOwner(EquipItemDB db)
        {
            var query = $@"update {Table} set {nameof(owner)}=@{nameof(owner)} where {nameof(item_no)}=@{nameof(item_no)}";
            var param = new DynamicParameters();
            param.Add(nameof(owner), db.owner);
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
