using System.Collections.Generic;


namespace GameDataTables
{
	public partial class DATA_CONSUME_ITEM_ROW
	{
		public int ID { get; private set; }
		public string NAME { get; private set; }
		public string CONSUME_TYPE { get; private set; }
		public int MAX_COUNT { get; private set; }
		public int USE_VALUE { get; private set; }
	}

	public partial class DATA_CONSUME_ITEM
	{
		public Dictionary<int, DATA_CONSUME_ITEM_ROW> Datas { get; private set; }
	}
}
