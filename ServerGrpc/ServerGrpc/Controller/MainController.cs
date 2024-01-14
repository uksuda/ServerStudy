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

        public override async Task<JoinRes> Join(JoinReq requset, ServerCallContext context)
        {
            if (string.IsNullOrEmpty(requset.Id) || string.IsNullOrEmpty(requset.Password))
            {
                _logger.LogError($"invalid Join : id or password is invalid {requset.Id} {requset.Password}");
                return new JoinRes
                {
                    Result = Network.Types.StatusCode.Invalid,
                };
            }

            if (string.IsNullOrEmpty(requset.Nickname) == true)
            {
                _logger.LogError($"invalid Join : nickname is invalid {requset.Nickname}");
                return new JoinRes
                {
                    Result = Network.Types.StatusCode.Invalid,
                };
            }

            var xtid = context.GetXtid();
            var res = await _service.Join(requset, xtid);
            return res;
        }

        public override async Task<LoginRes> Login(LoginReq requset, ServerCallContext context)
        {
            if (string.IsNullOrEmpty(requset.Id) || string.IsNullOrEmpty(requset.Password))
            {
                _logger.LogError($"invalid Login : id or password is invalid {requset.Id} {requset.Password}");
                return new LoginRes
                {
                    Result = Network.Types.StatusCode.Invalid,
                };
            }
            var session = context.GetClientSession();
            var res = await _service.Login(requset, session);
            return res;
        }

        public override async Task<UnaryData> UnaryDataSend(UnaryData requset, ServerCallContext context)
        {
            try
            {
                var session = context.GetClientSession();
                return await _service.UnaryDataSend(requset, session);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }

        public override async Task StreamOpen(IAsyncStreamReader<StreamMsg> reqStream, IServerStreamWriter<StreamMsg> resStream, ServerCallContext context)
        {
            // https://learn.microsoft.com/ko-kr/aspnet/core/grpc/services?view=aspnetcore-7.0

            var client = new ClientStream(reqStream, resStream, context);

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
