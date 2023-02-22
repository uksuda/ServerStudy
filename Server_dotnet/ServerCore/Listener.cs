using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;

namespace ServerCore
{
    public class Listener
    {
        private bool _isListening;
        private Socket _listenSocket;

        private CancellationTokenSource _cancelation;

        public Listener()
        {

        }
        
        public bool Init(IPEndPoint endPoint, int backLog)
        {
            _listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            _listenSocket.Bind(endPoint);
            _listenSocket.Listen(backLog);

            return true;
        }

        public bool Init(string host, int port, int backLog)
        {
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, port);

            return Init(endPoint, backLog);
        }

        public async Task StartAccept()
        {
            try
            {
                _cancelation = new CancellationTokenSource();
                CancellationToken token = _cancelation.Token;

                _isListening = true;
                while (_isListening)
                {
                    var socket = await _listenSocket.AcceptAsync(token).ConfigureAwait(false);
                    if (socket.Connected == false)
                    {
                        socket.Disconnect(false);
                        //socket.Shutdown(SocketShutdown.Both);
                        socket.Close();
                        continue;
                    }

                    var session = new Session(socket);
                    session.Index = SessionManager.Get().GenerateSessionIndex();
                    if (session == null)
                    {
                        continue;
                    }
                    SessionManager.Get().AddSession(session);

                    session.OnConnected();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void StopListening()
        {
            _isListening = false;
            _cancelation.Cancel();
        }
    }
}
