using System.Collections.Generic;


namespace GameDataTables
{
	public partial class DATA_ZONE_ROW
	{
		public int INDEX { get; private set; }
		public int GROUP_ID { get; private set; }
		public int X { get; private set; }
		public int Y { get; private set; }
		public string MOVE_LIST { get; private set; }
		public string SPECIAL_MOVE { get; private set; }
		public string MONSTER_GROUP_ID_LIST { get; private set; }
	}

	public partial class DATA_ZONE
	{
		public Dictionary<int, DATA_ZONE_ROW> Datas { get; private set; }
	}
}
