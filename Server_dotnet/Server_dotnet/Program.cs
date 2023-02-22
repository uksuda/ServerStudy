using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Network;
using Google.Protobuf;
using System.Threading;

namespace Server_dotnet
{    
    class Program
    {
        static void Main(string[] args)
        {
            //string host = Dns.GetHostName();
            //IPHostEntry ipHost = Dns.GetHostEntry(host);
            //IPAddress ipAddr = ipHost.AddressList[0];
            //IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            // Socket listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listenSocket.Bind(endPoint);
            listenSocket.Listen(5);

            Socket clientSocket = listenSocket.Accept();

            byte[] recvBuffer = new byte[1024];
            clientSocket.Receive(recvBuffer);

            Console.WriteLine(Encoding.UTF8.GetString(recvBuffer));
            clientSocket.Send(Encoding.UTF8.GetBytes("Hello Server"));

            Array.Clear(recvBuffer, 0, recvBuffer.Length);
            var recvSize = clientSocket.Receive(recvBuffer);
            
            MemoryStream s = new MemoryStream(recvBuffer, 0, recvSize);
            var packet = Packet.Parser.ParseFrom(s.ToArray());

            var e = packet.MessageCase;
            var sendPacket = new Packet { Id = 11, Second = new Second { Id = true } };

            Thread.Sleep(2000);
            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
            listenSocket.Close();
        }
    }
}
