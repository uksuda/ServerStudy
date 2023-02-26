using System.Collections.Concurrent;

namespace ServerGrpc.Grpc
{
    public class ClientManager
    {
        private readonly CancellationTokenSource _tokenSource;
        private readonly ConcurrentDictionary<int, ClientStream> _clientMap = new ConcurrentDictionary<int, ClientStream>();
        public ClientManager() 
        {
            _tokenSource = new CancellationTokenSource();
        }

        public void AddClient(ClientStream client)
        {
            _clientMap.TryAdd(_clientMap.Count, client);
        }
    }
}
