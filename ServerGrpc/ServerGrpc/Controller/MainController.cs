using Grpc.Core;
using Network.Main;
using ServerGrpc.Grpc;
using ServerGrpc.Services;

namespace ServerGrpc.Controller
{
    public class MainController : Main.MainBase
    {
        private readonly ILogger<MainController> _logger;
        private readonly MainService _service;

        private readonly ClientManager _clientManager;

        public MainController(
            ILogger<MainController> logger, 
            MainService service,
            ClientManager clientManager)
        {
            _service = service;
            _logger = logger;
            _clientManager = clientManager;
        }

        public override async Task<UnaryData> UnaryDataSend(UnaryData request, ServerCallContext context)
        {
            try
            {
                var session = context.GetClientSession();
                return await _service.UnaryDataSend(request, session);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public override async Task StreamOpen(IAsyncStreamReader<StreamMsg> requestStream, IServerStreamWriter<StreamMsg> responseStream, ServerCallContext context)
        {
            // https://learn.microsoft.com/ko-kr/aspnet/core/grpc/services?view=aspnetcore-7.0

            var client = new ClientStream(requestStream, responseStream, context);

            try
            {
                _logger.LogDebug("client connectted");

                await client.ReadAsync((data) =>
                {
                    //_logger.LogDebug($"recv data packet: {data.Packet}");
                    return _service.StreamDispatch(data, client, _clientManager);
                });
            }
            catch (RpcException e)
            {
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
            finally
            {
                client.Disconnect();
                _logger.LogDebug($"disconnected");
            }
        }
    }
}
