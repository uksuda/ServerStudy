using Grpc.Core;
using Grpc.Net.Client;
using Network.Main;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientGrpc.Client
{
    public class GrpcClient
    {
        private const string AUTHORIZATION = "Authorization";

        private GrpcChannel _channel;

        private Main.MainClient _mainClient;

        private Func<StreamMsg, bool> _callBack;

        private IAsyncStreamReader<StreamMsg> _streamReader;
        private IClientStreamWriter<StreamMsg> _streamWriter;

        private string _serverToken = string.Empty;

        private readonly CancellationTokenSource _tokenSource;

        public GrpcClient()
        {
            _tokenSource = new CancellationTokenSource();
        }

        public void SetStreamCallBack(Func<StreamMsg, bool> callBack)
        {
            _callBack = callBack;
        }

        public bool IsReadyChannel()
        {
            return (_channel != null && _channel.State == ConnectivityState.Ready);
        }

        public ConnectivityState GetChannelState()
        {
            if (_channel == null)
            {
                return ConnectivityState.Idle;
            }

            return _channel.State;
        }

        public bool InitChannel(string serverAddress)
        {
            var callCredentials = CallCredentials.FromInterceptor((ctx, meta) =>
            {
                if (string.IsNullOrEmpty(_serverToken) == false)
                {
                    meta.Add(AUTHORIZATION, _serverToken);
                }
                return Task.CompletedTask;
            });
            var channelCredentials = ChannelCredentials.Create(ChannelCredentials.Insecure, callCredentials);
            var channelOptions = new GrpcChannelOptions
            {
                //UnsafeUseInsecureChannelCallCredentials = true,
                Credentials = channelCredentials,
            };

            _channel = GrpcChannel.ForAddress(serverAddress, channelOptions);

            _mainClient = new Main.MainClient(_channel);
            return true;
        }

        public async Task<(JoinRes, string)> Join(JoinReq req)
        {
            try
            {
                var res = await _mainClient.JoinAsync(req);
                if (res.Result == Network.Types.StatusCode.Success)
                {
                    _serverToken = res.Token;
                }
                return (res, res.Result.ToString());
            }
            catch (Exception e)
            {
                //MessageBox.Show($"exception - {e}");
                return (null, $"exception - {e}");
            }
        }

        public async Task<(LoginRes, string)> Login(LoginReq req)
        {
            try
            {
                var res = await _mainClient.LoginAsync(req);
                if (res.Result == Network.Types.StatusCode.Success)
                {
                    _serverToken = res.Token;
                }
                return (res, res.Result.ToString());
            }
            catch (Exception e)
            {
                //MessageBox.Show($"exception - {e}");
                return (null, $"exception - {e}");
            }
        }

        public async Task<(UnaryData, string)> UnaryDataSend(UnaryData request)
        {
            try
            {
                var res = await _mainClient.UnaryDataSendAsync(request);
                return (res, string.Empty);
            }
            catch (Exception e)
            {
                //MessageBox.Show($"exception - {e}");
                return (null, $"exception - {e}");
            }
        }

        public async ValueTask SendMsg(StreamMsg data)
        {
            try
            {
                if (_tokenSource.IsCancellationRequested == false)
                {
                    await _streamWriter.WriteAsync(data);
                }
            }
            catch (OperationCanceledException e)
            {
                MessageBox.Show($"canceled exception - {e}");
            }
            catch (InvalidOperationException e)
            {
                MessageBox.Show($"invalid exception - {e}");
            }
            catch (RpcException e)
            {
                MessageBox.Show($"rpc exception - {e}");
            }
            catch (Exception e)
            {
                MessageBox.Show($"exception - {e}");
            }
        }

        public bool StreamOpen()
        {
            if (IsReadyChannel() == false)
            {
                MessageBox.Show($"invalid grpc channel");
                return false;
            }

            var call = _mainClient.StreamOpen(cancellationToken: _tokenSource.Token);
            _streamReader = call.ResponseStream;
            _streamWriter = call.RequestStream;

            try
            {
                _ = Task.Run(async () =>
                {
                    while (await _streamReader.MoveNext(_tokenSource.Token))
                    {
                        var data = _streamReader.Current;
                        if (_callBack != null)
                        {
                            _callBack.Invoke(data);
                        }
                    }
                    await _streamWriter.CompleteAsync();
                });
            }
            catch (RpcException e)
            {
                MessageBox.Show($"rpc exception {e.StatusCode} m: {e.Message}");
            }
            catch (Exception e)
            {
                MessageBox.Show($"exception. {e.Message}");
            }
            finally
            {
                MessageBox.Show($"disconnected");
            }
            return true;
        }
    }
}
