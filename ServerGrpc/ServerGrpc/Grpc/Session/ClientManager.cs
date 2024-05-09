using Game.Main;
using Game.Types;
using Grpc.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ServerGrpc.Infra;
using System.Collections.Concurrent;
using System.Security.Claims;

namespace ServerGrpc.Grpc.Session
{
    public class SessionManager
    {
        private readonly ILogger<SessionManager> _logger;
        private readonly CancellationTokenSource _tokenSource;

        private readonly ConcurrentDictionary<int, ClientSession> _clientMap = new ConcurrentDictionary<int, ClientSession>();
        private readonly ConcurrentDictionary<string, ClientSession> _clientIdMap = new ConcurrentDictionary<string, ClientSession>();

        public SessionManager(
            ILogger<SessionManager> logger,
            CancellationTokenSource tokenSource)
        {
            _tokenSource = CancellationTokenSource.CreateLinkedTokenSource(tokenSource.Token);
        }

        public async Task CheckToken(TokenValidatedContext ctx)
        {
            var xtid = ctx.HttpContext.GetXtid();
            if (string.IsNullOrEmpty(xtid) == true)
            {
                xtid = Guid.NewGuid().ToString("N");
            }

            if (ctx.SecurityToken.ValidTo > DateTime.UtcNow)
            {
                ctx.HttpContext.SetContextErr(ResultCode.TokenExpire);
                ctx.HttpContext.SetXtid(xtid);
                return;
            }

            var prin = ctx.Principal;
            var mberNoStr = prin.FindFirstValue(JwtTokenBuilder.MBER_NO);
            if (int.TryParse(mberNoStr, out int mberNo) == false)
            {
                ctx.HttpContext.SetContextErr(ResultCode.TokenInvalid);
                ctx.HttpContext.SetXtid(xtid);
                return;
            }

            var session = new GameSession(mberNo, xtid);

            ctx.HttpContext.SetSession(session);

            _logger.LogDebug($"CheckToken called. mber {mberNo}");
            await Task.CompletedTask;
        }

        public ClientSession AddClient(IAsyncStreamReader<StreamData> reqStream, IServerStreamWriter<StreamData> resStream, ServerCallContext context)
        {
            ClientSession client = new ClientSession(reqStream, resStream, context);
            if (_clientMap.TryAdd(client.Mber, client) == false)
            {
                return null;
            }
            return client;
        }

        public bool RemoveClient(int mberNo)
        {
            _clientMap.TryRemove(mberNo, out _);
            return true;
        }
    }
}
