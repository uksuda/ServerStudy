using Grpc.Core;
using Grpc.Net.Client;
using network.main;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ClientGrpc.Client
{
    public class GrpcClient
    {
        private GrpcChannel _channel;
        private Main.MainClient _mainClient;

        private Func<StreamMsg, bool> _callBack;

        private IAsyncStreamReader<StreamMsg> _streamReader;
        private IClientStreamWriter<StreamMsg> _streamWriter;

        private Grpc.Core.Metadata _metaData;

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
            return (_channel != null && _metaData != null);
        }

        public ConnectivityState GetChannelState()
        {
            if (_channel == null)
            {
                return ConnectivityState.Idle;
            }

            return _channel.State;
        }

        public bool InitChannel(string serverAddress, string id, string password)
        {
            //var callCredentials = CallCredentials.FromInterceptor(((context, metadata) =>
            //{
            //    metadata.Add("id", id);
            //    metadata.Add("password", password);
            //    return Task.CompletedTask;
            //}));

            //var channelCredentials = ChannelCredentials.Create(new SslCredentials(), callCredentials);
            //var channelOptions = new GrpcChannelOptions
            //{
            //    //UnsafeUseInsecureChannelCallCredentials = true,
            //    Credentials = channelCredentials
            //};

            //_channel = GrpcChannel.ForAddress(serverAddress, channelOptions);

            _channel = GrpcChannel.ForAddress(serverAddress);
            _mainClient = new Main.MainClient(_channel);

            _metaData = new Metadata();
            _metaData.Add("id", id);
            _metaData.Add("password", password);
            return true;
        }

        public async Task<JoinRes> Join(JoinReq req)
        {
            try
            {
                var res = await _mainClient.JoinAsync(req);
                return res;
            }
            catch (Exception e)
            {
                MessageBox.Show($"exception - {e}");
            }
            return null;
        }

        public async Task<LoginRes> Login(LoginReq req)
        {
            try
            {
                var res = await _mainClient.LoginAsync(req);
                return res;
            }
            catch (Exception e)
            {
                MessageBox.Show($"exception - {e}");
            }
            return null;
        }

        public async Task<UnaryData> UnaryDataSend(UnaryData request)
        {
            try
            {
                var res = await _mainClient.UnaryDataSendAsync(request, _metaData);
                return res;
            }
            catch (Exception e)
            {
                MessageBox.Show($"exception - {e}");
            }
            return null;
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
            if (_metaData == null)
            {
                MessageBox.Show("stream must need metadata");
                return false;
            }

            var call = _mainClient.StreamOpen(_metaData, null, _tokenSource.Token);
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
                throw;
            }
            catch (Exception e)
            {
                MessageBox.Show($"exception. {e.Message}");
                throw;
            }
            return true;
        }
    }
}
