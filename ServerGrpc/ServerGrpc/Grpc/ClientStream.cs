using Grpc.Core;
using Game.Main;
using ServerGrpc.Common;

namespace ServerGrpc.Grpc
{
    public class ClientStream
    {
        public string XTID => _session.XTID;
        public string ID => _session.ID;
        
        private readonly IAsyncStreamReader<StreamMsg> _reader;
        private readonly IServerStreamWriter<StreamMsg> _writer;

        private readonly ServerCallContext _context;
        private readonly ClientSession _session;

        private readonly CancellationTokenSource _tokenSource;

        private Action<StreamMsg> _callback;
        private int _clientIndex;

        public ClientStream(IAsyncStreamReader<StreamMsg> request, IServerStreamWriter<StreamMsg> response, ServerCallContext context)
        {
            _reader = request;
            _writer = response;
            _context = context;

            _session = _context.GetClientSession();
            
            _tokenSource = new CancellationTokenSource();
        }

        public void SetClientIndex(int index)
        {
            _clientIndex = index;
        }

        public async ValueTask ReadAsync(Func<StreamMsg, Task<bool>> msgCallBack)
        {
            await Task.Run(async () =>
            {
                while (await _reader.MoveNext(_tokenSource.Token))
                {
                    var data = _reader.Current;
                    if (msgCallBack != null)
                    {
                        await msgCallBack.Invoke(data);
                    }
                }
            });
        }

        public void SetMSgCallBack(Action<StreamMsg> callBack)
        {
            _callback = callBack;
        }

        public async ValueTask StartReadAsync()
        {
            await Task.Run(async () =>
            {
                while (await _reader.MoveNext(_tokenSource.Token))
                {
                    var data = _reader.Current;
                    if (_callback != null)
                    {
                        _callback.Invoke(data);
                    }
                }
            });
        }

        public async ValueTask SendMsg(StreamMsg data)
        {
            if (_tokenSource.IsCancellationRequested == false)
            {
                await _writer.WriteAsync(data);
            }
        }

        public void Disconnect()
        {
            if (_tokenSource.IsCancellationRequested == false)
            {
                _tokenSource.Cancel();
            }
        }
    }
}
