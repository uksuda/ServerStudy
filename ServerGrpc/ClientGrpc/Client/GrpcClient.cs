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

        private Func<StreamData, bool> _callBack;

        private IAsyncStreamReader<StreamData> _streamReader;
        private IClientStreamWriter<StreamData> _streamWriter;

        private readonly CancellationTokenSource _tokenSource;

        public GrpcClient()
        {
            _tokenSource = new CancellationTokenSource();
        }

        public void SetStreamCallBack(Func<StreamData, bool> callBack)
        {
            _callBack = callBack;
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
            _channel = GrpcChannel.ForAddress(serverAddress);
            _mainClient = new Main.MainClient(_channel);
            return true;
        }

        public async Task<UnaryData> UnaryDataSend(UnaryData request)
        {
            try
            {
                var res = await _mainClient.UnaryDataSendAsync(request);
                return res;
            }
            catch (Exception e)
            {
                MessageBox.Show($"exception - {e}");
            }

            return null;
        }

        public async ValueTask SendMsg(StreamData data)
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

        public bool StreamOpen(Grpc.Core.Metadata metaData = null)
        {
            Grpc.Core.Metadata meta = (metaData == null) ? Grpc.Core.Metadata.Empty : metaData;

            var call = _mainClient.StreamOpen(meta, null, _tokenSource.Token);
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
