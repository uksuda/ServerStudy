using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;
using Network;

namespace ServerCore
{
    public class Session
    {
        private Socket _socket;

        private object _lock = new object();

        private CancellationTokenSource _cancellation;

        private RecvBuffer _recvBuffer = new RecvBuffer(1024);
        private SendBuffer _sendBuffer = new SendBuffer();

        public int Index { get; set; }
        public bool Disconnected { get; private set; }

        public Session(Socket socket)
        {
            _socket = socket;
            _cancellation = new CancellationTokenSource();

            _recvBuffer.Clear();
            _sendBuffer.Clear();
        }

        public async Task<bool> OnRecv()
        {
            var recvBuffer = _recvBuffer.GetAvailableBuffer();
            if (recvBuffer == null)
            {
                return false;
            }

            var recvSize = await _socket.ReceiveAsync(recvBuffer, SocketFlags.None, _cancellation.Token);
            _recvBuffer.OnRecv(recvSize);

            // packet handler

            return true;
        }

        public async Task OnSend(Packet p, bool flush = false)
        {
            _sendBuffer.AddSendList(p.ToByteArray());

            if (flush == true)
            {
                while (_sendBuffer.PendingList.Count > 0)
                {
                    await OnSendFlush();
                }
            }
        }

        public async Task<bool> OnSendFlush()
        {
            var sendBytes = await _socket.SendAsync(_sendBuffer.PendingList, SocketFlags.None);
            _sendBuffer.OnSend(sendBytes);
            return true;
        }

        public void OnConnected()
        {
            Console.WriteLine($"session conntected ip {_socket.RemoteEndPoint}");
        }

        public void Disconnect()
        {
            lock(_lock)
            {
                if (Disconnected)
                    return;

                Disconnected = true;
            }

            Console.WriteLine($"socket disconnected {_socket.RemoteEndPoint}");
            _cancellation.Cancel();

            Close();
        }

        public void Close(bool force = false)
        {
            if (force == true)
            {
                _socket.Shutdown(SocketShutdown.Both);
            }
            else
            {
                _socket.Disconnect(false);
            }

            _socket.Close();

            _recvBuffer.Clear();
            _sendBuffer.Clear();
        }
    }
}
