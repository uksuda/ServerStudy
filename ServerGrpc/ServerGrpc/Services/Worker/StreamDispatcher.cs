using Game.Main;
using ServerGrpc.Common;
using ServerGrpc.Grpc.Session;

namespace ServerGrpc.Services.Worker
{
    public class StreamDispatcher
    {
        private readonly ILogger<StreamDispatcher> _logger;

        public StreamDispatcher(
            ILogger<StreamDispatcher> logger)
        {
            _logger = logger;
        }

        public bool StreamDispatch(ClientSession client, StreamData data)
        {
            switch (data.Packet)
            {
                case Game.Types.StreamPacket.ConnectReq:
                    {
                        throw ErrorHandler.Error_Stream(Game.Types.ResultCode.InvalidReqParam, "error test", data.Packet);
                    }
                    break;
                default:
                    break;
            }
            return true;
        }

        private void Stream_ConnectReq(ClientSession client, StreamData data)
        {

        }
    }
}
