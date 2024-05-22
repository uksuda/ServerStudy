using System.Collections.Generic;


namespace GameDataTables
{
	public partial class DATA_DROP_ITEM_ROW
	{
		public int ID { get; private set; }
		public long GROUP_ID { get; private set; }
		public int ITEM_ID { get; private set; }
		public int RATE { get; private set; }
	}

	public partial class DATA_DROP_ITEM
	{
		public Dictionary<int, DATA_DROP_ITEM_ROW> Datas { get; private set; }
	}
}
