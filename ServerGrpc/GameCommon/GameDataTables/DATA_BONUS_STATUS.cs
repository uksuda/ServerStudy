using System.Collections.Generic;


namespace GameDataTables
{
	public partial class DATA_BONUS_STATUS_ROW
	{
		public int ID { get; private set; }
		public int GROUP_ID { get; private set; }
		public string STATUS_TYPE { get; private set; }
		public int MIN_VALUE { get; private set; }
		public int MAX_VALUE { get; private set; }
	}

	public partial class DATA_BONUS_STATUS
	{
		public Dictionary<int, DATA_BONUS_STATUS_ROW> Datas { get; private set; }
	}
}
