using System.Collections.Concurrent;

namespace ServerGrpc.Grpc
{
    public class ClientManager
    {
        private readonly CancellationTokenSource _tokenSource;
        private readonly ConcurrentDictionary<int, ClientStream> _clientMap = new ConcurrentDictionary<int, ClientStream>();

        private readonly ConcurrentDictionary<string, ClientStream> _clientIdMap = new ConcurrentDictionary<string, ClientStream>();
        public ClientManager() 
        {
            _tokenSource = new CancellationTokenSource();
        }

        public void AddClient(ClientStream client)
        {
            _clientMap.TryAdd(_clientIdMap.Count, client);
        }
    }
}
