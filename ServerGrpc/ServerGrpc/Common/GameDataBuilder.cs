using GameCommon.DataBuilder;
using GameDataTables;

namespace ServerGrpc.Common
{
    public class GameDataBuilder
    {
        private readonly ILogger<GameDataBuilder> _logger;
        private DataBuilder _dataBuilder;

        public GameDataBuilder(ILogger<GameDataBuilder> logger, AppSettings appSetting)
        {
            _logger = logger;
            _dataBuilder = new DataBuilder(appSetting.DataPath);
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
