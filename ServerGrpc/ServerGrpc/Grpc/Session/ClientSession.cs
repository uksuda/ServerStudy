using Grpc.Core;
using Game.Main;
using ServerGrpc.Logger;
using System.Reflection;

namespace ServerGrpc.Grpc.Session
{
    public class GameSession
    {
        public int MberNo { get; private set; }
        public string Xtid { get; private set; }
        public string Xtid_Stream { get; private set; }

        public GameSession(int mberNo, string xtid)
        {
            MberNo = mberNo;
            Xtid = xtid;
            Xtid_Stream = string.Empty;
        }

        public void SetXtidStream(string xtid)
        {
            Xtid_Stream = xtid;
        }
    }

    public class ClientSession
    {
        public string Xtid => _session.Xtid;
        public string XtidStream => _session.Xtid_Stream;
        public int Mber => _session.MberNo;

        private readonly ILogger _logger = AppLogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IAsyncStreamReader<StreamData> _reader;
        private readonly IServerStreamWriter<StreamData> _writer;

        private readonly ServerCallContext _context;
        private readonly GameSession _session;

        private readonly CancellationTokenSource _tokenSource;

        private int _clientIndex;

        public ClientSession(IAsyncStreamReader<StreamData> request, IServerStreamWriter<StreamData> response, ServerCallContext context)
        {
            _reader = request;
            _writer = response;
            _context = context;

            _session = _context.GetSession();

            _tokenSource = new CancellationTokenSource();
        }

        public void SetClientIndex(int index)
        {
            _clientIndex = index;
        }

        public async ValueTask ReadAsync(Func<StreamData, bool> msgCallBack)
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

        public async Task<bool> SendMsg(StreamData data)
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

        public async ValueTask SendAndDisconnect(StreamData data)
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
