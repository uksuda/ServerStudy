using System.Collections.Generic;


namespace GameDataTables
{
	public partial class DATA_EQUIP_ITEM_ROW
	{
		public int ID { get; private set; }
		public string NAME { get; private set; }
		public string GRADE_TYPE { get; private set; }
		public string EQUIP_TYPE { get; private set; }
		public int BASE_ATT { get; private set; }
		public int BASE_DEF { get; private set; }
		public int BONUS_ATT { get; private set; }
		public int BONUS_DEF { get; private set; }
		public int BONUS_STATUS_GROUP_ID { get; private set; }
	}

	public partial class DATA_EQUIP_ITEM
	{
		public Dictionary<int, DATA_EQUIP_ITEM_ROW> Datas { get; private set; }
	}
}
