using Grpc.Core;
using Network.Main;
using Network.Unary;
using ServerGrpc.Common;
using ServerGrpc.Grpc;
using System.ComponentModel;
using System.Reflection;

namespace ServerGrpc.Services
{
    public class MainService
    {
        private readonly ILogger<MainService> _logger;
        public MainService(ILogger<MainService> logger)
        {
            _logger = logger;
        }

        #region Join & Login
        public async Task<JoinRes> Join(JoinReq requset, string xtid)
        {

            return default;
        }

        public async Task<LoginRes> Login(LoginReq requset, ClientSession session)
        {
            return default;
        }
        #endregion

        #region Unary Data
        public async Task<UnaryData> UnaryDataSend(UnaryData requset, ClientSession session)
        {
            if (requset != null)
            {
                _logger.LogDebug($"{MethodBase.GetCurrentMethod()} - {requset}");
            }

            UnaryData response = null;
            switch (requset.Type)
            {
                case Network.Types.UnaryDataType.CommandReq:
                    response = await UnaryDispatch_CommandReq(requset.CommandReq, session);
                    break;
                default:
                    throw new InvalidEnumArgumentException($"invalid request type {requset.Type}");
            }

            return response;
        }

        private async Task<UnaryData> UnaryDispatch_CommandReq(Unary_CommandReq request, ClientSession session)
        {
            return default;
        }
        #endregion

        #region StreamData
        public async Task<bool> StreamDispatch(StreamMsg data, ClientStream client, ClientManager manager)
        {
            switch (data.Packet)
            {
                case Network.Types.StreamPacket.Disconnected:
                    break;
                case Network.Types.StreamPacket.MessageSend:
                    break;
                default:
                    throw new InvalidEnumArgumentException($"invalid stream data {data.Packet}");
            }

            return true;
        }
        #endregion
    }
}