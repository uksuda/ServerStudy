using System.Collections.Generic;


namespace GameDataTables
{
	public partial class DATA_JOB_ARCHER_ROW
	{
		public int LEVEL { get; private set; }
		public int STR { get; private set; }
		public int DEX { get; private set; }
		public int VITAL { get; private set; }
		public int ENERGY { get; private set; }
		public int ADD_HP { get; private set; }
		public int ADD_MP { get; private set; }
	}

	public partial class DATA_JOB_ARCHER
	{
		public Dictionary<int, DATA_JOB_ARCHER_ROW> Datas { get; private set; }
	}
}
