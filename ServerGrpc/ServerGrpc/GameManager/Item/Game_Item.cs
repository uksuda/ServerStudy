using ServerGrpc.DB.Table;

namespace ServerGrpc.GameManager.Item
{
    public abstract class Game_Item
    {
        public abstract long ItemNo { get; }
        public abstract int ItemId { get; }

        protected readonly Game.Types.ItemType _itemType;

        public Game_Item(Game.Types.ItemType itemType)
        {
            _itemType = itemType;
        }
    }

    public class Game_EquipItem : Game_Item
    {
        public override long ItemNo => _equip.item_no;
        public override int ItemId => _equip.id;

        private readonly EquipItemDB _equip;
        //private readonly List<StatusInfo> _status;

        public Game_EquipItem(EquipItemDB db)
            : base(Game.Types.ItemType.Equip)
        {
            _equip = db;
        }

        public Game.Common.EquipInfo ToProto()
        {
            return new Game.Common.EquipInfo
            {
                No = _equip.item_no,
                ItemId = _equip.id,
                //Equip = (ItemEquipType)

            };
        }

        private IEnumerable<Game.Common.StatusInfo> GetStatus()
        {
            var r = new List<Game.Common.StatusInfo>();
            if (_equip == null)
            {
                return r;
            }

            // att
            if (_equip.att > 0)
            {
                r.Add(new Game.Common.StatusInfo
                {
                    Status = Game.Types.StatusType.Att,
                    Value = _equip.att,
                });
            }
            // def
            if (_equip.def > 0)
            {
                r.Add(new Game.Common.StatusInfo
                {
                    Status = Game.Types.StatusType.Def,
                    Value = _equip.def,
                });
            }
            // status_1
            if (_equip.bonus_status_type_1 != (int)Game.Types.StatusType.None && _equip.bonus_status_value_1 > 0)
            {
                r.Add(new Game.Common.StatusInfo
                {
                    Status = (Game.Types.StatusType)_equip.bonus_status_type_1,
                    Value = _equip.bonus_status_value_1,
                });
            }
            // status_2
            if (_equip.bonus_status_type_2 != (int)Game.Types.StatusType.None && _equip.bonus_status_value_2 > 0)
            {
                r.Add(new Game.Common.StatusInfo
                {
                    Status = (Game.Types.StatusType)_equip.bonus_status_type_2,
                    Value = _equip.bonus_status_value_2,
                });
            }
            // status_3
            if (_equip.bonus_status_type_3 != (int)Game.Types.StatusType.None && _equip.bonus_status_value_3 > 0)
            {
                r.Add(new Game.Common.StatusInfo
                {
                    Status = (Game.Types.StatusType)_equip.bonus_status_type_3,
                    Value = _equip.bonus_status_value_3,
                });
            }
            return r;
        }
    }

    public class Game_ConsumeItem : Game_Item
    {
        public override long ItemNo => _consume.item_no;
        public override int ItemId => _consume.id;

        private readonly ConsumeItemDB _consume;

        public Game_ConsumeItem(ConsumeItemDB db)
            : base(Game.Types.ItemType.Consume)
        {

        }

        public Game.Common.ConsumeItem ToProto()
        {
            var r = new Game.Common.ConsumeItem
            {
                No = ItemNo,
                ItemId = this.ItemId,
            };
            return r;
        }
    }
}
