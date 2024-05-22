using System.Collections.Generic;


namespace GameDataTables
{
	public partial class DATA_MONSTER_ROW
	{
		public int ID { get; private set; }
		public int GROUP_ID { get; private set; }
		public string GRADE { get; private set; }
		public string NAME { get; private set; }
		public int LEVEL { get; private set; }
		public int ATT { get; private set; }
		public int DEF { get; private set; }
		public int GEN_RATE { get; private set; }
		public int BASE_DROP_RATE { get; private set; }
		public int SPECIAL_DROP_RATE { get; private set; }
		public int BASE_DROP_GROUP_ID { get; private set; }
		public int SPECIAL_DROP_GROUP_ID { get; private set; }
	}

	public partial class DATA_MONSTER
	{
		public Dictionary<int, DATA_MONSTER_ROW> Datas { get; private set; }
	}
}
