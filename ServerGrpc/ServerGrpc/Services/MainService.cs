using Grpc.Core;
using network.main;
using network.unary;
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

        #region Unary Data
        public async Task<UnaryData> UnaryDataSend(UnaryData request, ClientSession session)
        {
            if (request != null)
            {
                _logger.LogDebug($"{MethodBase.GetCurrentMethod()} - {request}");
            }

            UnaryData response = null;
            switch (request.Type)
            {
                case network.types.UnaryDataType.JoinReq:
                    response = await UnaryDispatch_JoinReq(request.JoinReq, session);
                    break;
                case network.types.UnaryDataType.CommandReq:
                    response = await UnaryDispatch_CommandReq(request.CommandReq, session);
                    break;
                default:
                    throw new InvalidEnumArgumentException($"invalid request type {request.Type}");
            }

            return response;
        }

        private async Task<UnaryData> UnaryDispatch_JoinReq(Unary_JoinReq request, ClientSession session)
        {
            return default;
        }

        private async Task<UnaryData> UnaryDispatch_CommandReq(Unary_CommandReq requesyt, ClientSession session)
        {
            return default;
        }
        #endregion

        #region StreamData
        public async Task<bool> StreamDispatch(StreamMsg data, ClientStream client, ClientManager manager)
        {
            switch (data.Packet)
            {
                case network.types.StreamPacket.Disconnected:
                    break;
                case network.types.StreamPacket.MessageSend:
                    break;
                default:
                    throw new InvalidEnumArgumentException($"invalid stream data {data.Packet}");
            }

            return true;
        }
        #endregion
    }
}