using System.Collections.Generic;


namespace GameDataTables
{
	public partial class DATA_CHARACTER_ROW
	{
		public int LEVEL { get; private set; }
		public long EXP { get; private set; }
		public int MAX_HP { get; private set; }
		public int MAX_MP { get; private set; }
		public bool TEST_1 { get; private set; }
		public string TEST_2 { get; private set; }
	}

	public partial class DATA_CHARACTER
	{
		public Dictionary<int, DATA_CHARACTER_ROW> Datas { get; private set; }
	}
}
