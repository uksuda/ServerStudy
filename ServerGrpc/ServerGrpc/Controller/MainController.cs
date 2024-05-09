using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Game.Main;
using ServerGrpc.Common;
using ServerGrpc.Grpc;
using ServerGrpc.Services;
using ServerGrpc.Grpc.Session;
using System.Text;

namespace ServerGrpc.Controller
{
    [Authorize]
    public class MainController : Main.MainBase
    {
        private readonly ILogger<MainController> _logger;
        private readonly MainService _service;

        private readonly SessionManager _sessionManager;

        public MainController(
            ILogger<MainController> logger, 
            MainService service,
            SessionManager sessionManager)
        {
            _service = service;
            _logger = logger;
            _sessionManager = sessionManager;
        }

        [AllowAnonymous]
        public override async Task<JoinRes> Join(JoinReq requset, ServerCallContext context)
        {
            try
            {
                if (string.IsNullOrEmpty(requset.Id) || string.IsNullOrEmpty(requset.Password))
                {
                    throw ErrorHandler.Error(Game.Types.ResultCode.InvalidReqParam, "invalid id & password");
                }

                var xtid = context.GetXtid();
                var res = await _service.Join(requset, xtid);
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [AllowAnonymous]
        public override async Task<LoginRes> Login(LoginReq requset, ServerCallContext context)
        {
            try
            {
                if (string.IsNullOrEmpty(requset.Id) || string.IsNullOrEmpty(requset.Password))
                {
                    throw ErrorHandler.Error(Game.Types.ResultCode.InvalidReqParam, "invalid id & password");
                }

                var xtid = context.GetXtid();
                var res = await _service.Login(requset, xtid);
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override async Task<UnaryData> UnaryDataSend(UnaryData requset, ServerCallContext context)
        {
            try
            {
                var session = context.GetSession();
                return await _service.UnaryDataSend(requset, session);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override async Task StreamOpen(IAsyncStreamReader<StreamData> reqStream, IServerStreamWriter<StreamData> resStream, ServerCallContext context)
        {
            // https://learn.microsoft.com/ko-kr/aspnet/core/grpc/services?view=aspnetcore-7.0

            ClientSession client = null;
            try
            {
                client = _sessionManager.AddClient(reqStream, resStream, context);
                if (client == null)
                {
                    throw ErrorHandler.Error(Game.Types.ResultCode.UnknownError, "stream open fail");
                }

                await _service.StreamDispatch(client);
            }
            catch (Exception e)
            {
                var sb = new StringBuilder();
                sb.AppendLine($" - {e.Message}");
                var ie = e.InnerException;
                if (ie != null)
                {
                    //sb.AppendLine($"inner - {ie.Message}");
                    sb.AppendLine($"inner - {ie.Message}");
                }
                _logger.LogError($"StreamOpen : {sb}");
            }
            finally
            {
                client.Disconnect();
                _logger.LogDebug($"disconnected");
            }
        }
    }
}
