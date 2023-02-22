using Network;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ServerCore
{
    public class PacketHandler
    {
        #region SINGLETON
        private static PacketHandler _instance = null;
        public static PacketHandler Get()
        {
            if (_instance == null)
            {
                _instance = new PacketHandler();
            }

            return _instance;
        }
        #endregion

        private readonly ConcurrentQueue<Packet> _packetsQueue = new ConcurrentQueue<Packet>();
        private readonly ConcurrentDictionary<Packet.MessageOneofCase, Action<Session, Packet>> _packetHandleFunc = new ConcurrentDictionary<Packet.MessageOneofCase, Action<Session, Packet>>();

        public bool Initialize()
        {
            _packetsQueue.Clear();
            _packetHandleFunc.Clear();

            return true;
        }

        public void AddPacketHandleFunc(Packet.MessageOneofCase id, Action<Session, Packet> action)
        {
            if (_packetHandleFunc.ContainsKey(id) == false)
            {
                _packetHandleFunc.TryAdd(id, action);
            }
        }
    }
}
