using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Network;
using Google.Protobuf;
using System.Runtime.Serialization.Formatters.Binary;

namespace DummyClient
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

            //Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(endPoint);

            socket.Send(Encoding.UTF8.GetBytes("Hello Client"));

            var recv = new byte[1024];
            socket.Receive(recv);

            Console.WriteLine(Encoding.UTF8.GetString(recv));

            var send = new byte[256];
            var p = new Packet { Id = 10, First = new First { Id = 1, Dd = "asd" } };

            socket.SendAsync(p.ToByteArray(), SocketFlags.None);

            BinaryFormatter bf = new BinaryFormatter();
            
            var temp = new MemoryStream();
            socket.ReceiveAsync(temp.ToArray(), SocketFlags.None);
            var pp = Packet.Parser.ParseFrom(temp);
            
            byte[] recvBuffer = new byte[1024];
            var recvSize = socket.Receive(recvBuffer);
            var recvSpan = new Span<byte>(recvBuffer, 0, recvSize).ToArray();
            var recvPacket = recvSpan.ToObject<Packet>();

            Console.WriteLine(recvBuffer.ToString());

            socket.Shutdown(SocketShutdown.Both);
            socket.Close();

            // Console.WriteLine("Hello World!");
        }
    }
}
