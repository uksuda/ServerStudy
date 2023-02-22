using Network;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    public class Connector
    {
        private Session _session;

        public Connector()
        {
            
        }

        public bool Init(IPEndPoint endPoint)
        {
            Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(endPoint);

            _session = new Session(socket);

            return true;
        }

        public bool Init(string ipString, int port)
        {
            IPAddress ipAddr = IPAddress.Parse(ipString);
            IPEndPoint endPoint = new IPEndPoint(ipAddr, port);
            return Init(endPoint);
        }

        public async Task<bool> OnRecv()
        {
            return await _session?.OnRecv();
        }

        public async Task OnSend(Packet p, bool flush = false)
        {
            await _session?.OnSend(p, flush);
        }
    }
}
