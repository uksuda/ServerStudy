using Grpc.Core;
using network.main;
using ServerGrpc.Grpc;
using ServerGrpc.Services;

namespace ServerGrpc.Controller
{
    public class MainController : Main.MainBase
    {
        private readonly ILogger<MainController> _logger;
        private readonly MainService _service;

        public MainController(ILogger<MainController> logger, MainService service)
        {
            _service = service;
            _logger = logger;
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

        public override async Task StreamOpen(IAsyncStreamReader<StreamData> requestStream, IServerStreamWriter<StreamData> responseStream, ServerCallContext context)
        {
            // https://learn.microsoft.com/ko-kr/aspnet/core/grpc/services?view=aspnetcore-7.0
            
            try
            {
                var client = new ClientStream(requestStream, responseStream, context);
                _logger.LogDebug("client connectted");

                await client.ReadAsync((data) =>
                {
                    _logger.LogDebug($"recv data packet: {data.Packet}");
                    return true;
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
                _logger.LogDebug($"disconnected");
            }
        }
    }
}
