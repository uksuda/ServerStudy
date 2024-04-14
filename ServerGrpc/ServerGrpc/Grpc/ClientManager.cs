using System.Collections.Concurrent;

namespace ServerGrpc.Grpc
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

        public void AddClient(ClientStream client)
        {
            _clientMap.TryAdd(_clientIdMap.Count, client);
        }
    }
}
