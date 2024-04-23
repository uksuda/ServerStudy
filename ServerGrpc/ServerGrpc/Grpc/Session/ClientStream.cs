using Grpc.Core;
using Game.Main;
using ServerGrpc.Logger;
using System.Reflection;

namespace ServerGrpc.Grpc.Session
{
    public class ClientStream
    {
        public string XTID => _session.Xtid;
        public string XTID_STREAM => _session.Xtid_Stream;
        public int MBER => _session.MberNo;

        private readonly ILogger _logger = AppLogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IAsyncStreamReader<StreamMsg> _reader;
        private readonly IServerStreamWriter<StreamMsg> _writer;

        private readonly ServerCallContext _context;
        private readonly ClientSession _session;

        private readonly CancellationTokenSource _tokenSource;

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

        public async ValueTask ReadAsync(Func<StreamMsg, bool> msgCallBack)
        {
            await Task.Run(async () =>
            {
                while (await _reader.MoveNext(_tokenSource.Token))
                {
                    var data = _reader.Current;
                    if (msgCallBack != null)
                    {
                        msgCallBack.Invoke(data);
                    }
                }
            });
        }

        public async Task<bool> SendMsg(StreamMsg data)
        {
            if (data.Packet == Game.Types.StreamPacket.None)
            {
                return false;
            }

            if (_tokenSource.IsCancellationRequested == false)
            {
                try
                {
                    await _writer.WriteAsync(data);
                    return true;
                }
                catch (RpcException e)
                {
                    _logger.LogError($"send err - mber: {_session.MberNo} : {e}");
                }
                catch (Exception e)
                {
                    _logger.LogError($"send err - {e}");
                }                
            }
            return false;
        }

        public async ValueTask SendAndDisconnect(StreamMsg data)
        {
            await SendMsg(data).ContinueWith(t => { Disconnect(); });
        }

        public void Disconnect()
        {
            if (_tokenSource.IsCancellationRequested == false)
            {
                _tokenSource.Cancel();
            }
            _tokenSource.Dispose();
        }
    }
}
