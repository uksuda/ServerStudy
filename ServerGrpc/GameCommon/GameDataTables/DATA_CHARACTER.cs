using System.Collections.Generic;


namespace GameDataTables
{
	public partial class DATA_CHARACTER_ROW
	{
		public int LEVEL { get; private set; }
		public long EXP { get; private set; }
		public int MAX_HP { get; private set; }
		public int MAX_MP { get; private set; }
		public int ATT { get; private set; }
		public int DEF { get; private set; }
	}

	public partial class DATA_CHARACTER
	{
		public Dictionary<int, DATA_CHARACTER_ROW> Datas { get; private set; }
	}
}
