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

        public void Start()
        {
            while (_tokenSource.IsCancellationRequested == false)
            {
                Thread.Sleep(5000);
                foreach (var client in _clientMap.Values)
                {
                    _ = client.ReadAsync((data) =>
                    {
                        //_logger.LogDebug($"recv data packet: {data.Packet}");
                        Console.WriteLine($"recv data packet: {data.Packet}");
                        return true;
                    });
                }
                break;
            }
        }
    }
}
