using Game.Types;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ServerGrpc.Infra;
using System.Collections.Concurrent;
using System.Security.Claims;

namespace ServerGrpc.Grpc.Session
{
    public class ClientManager
    {
        private readonly ILogger<ClientManager> _logger;
        private readonly CancellationTokenSource _tokenSource;

        private readonly ConcurrentDictionary<int, ClientStream> _clientMap = new ConcurrentDictionary<int, ClientStream>();
        private readonly ConcurrentDictionary<string, ClientStream> _clientIdMap = new ConcurrentDictionary<string, ClientStream>();

        public ClientManager(
            ILogger<ClientManager> logger,
            CancellationTokenSource tokenSource)
        {
            _tokenSource = CancellationTokenSource.CreateLinkedTokenSource(tokenSource.Token);
        }

        public async Task CheckToken(TokenValidatedContext ctx)
        {
            if (ctx.SecurityToken.ValidTo > DateTime.UtcNow)
            {
                ctx.HttpContext.SetContextErr(ResultCode.TokenExpire);
                return;
            }

            var prin = ctx.Principal;
            var mberNoStr = prin.FindFirstValue(JwtTokenBuilder.MBER_NO);
            if (int.TryParse(mberNoStr, out int mberNo) == false)
            {
                ctx.HttpContext.SetContextErr(ResultCode.TokenInvalid);
                return;
            }

            var xtid = Guid.NewGuid().ToString("N");
            var session = new ClientSession(mberNo, xtid);

            ctx.HttpContext.SetClientSession(session);
            await Task.CompletedTask;
        }

        public bool AddClient(ClientStream client)
        {
            _clientMap.TryAdd(_clientIdMap.Count, client);
            return true;
        }

        public bool RemoveClient()
        {
            return true;
        }
    }
}
