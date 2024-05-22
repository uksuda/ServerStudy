using GameCommon.DataBuilder;
using GameDataTables;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientGrpc.Common
{
    // Client data builder
    public class GameDataBuilder
    {
        #region SINGLETONE
        private static GameDataBuilder _instance;
        public static GameDataBuilder Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameDataBuilder();
                }
                return _instance;
            }
        }
        #endregion

        private DataBuilder _dataBuilder;

        public GameDataBuilder()
        {
            _dataBuilder = new DataBuilder(ClientConst.GAME_DATA_PATH);
            var vvv = new string[] { };
            vvv.ElementAt(1);
        }

        public void BuildData()
        {
            _dataBuilder.BuildData();
        }

        public GameDataManager GetData()
        {
            return _dataBuilder.GetData();
        }
    }
}
