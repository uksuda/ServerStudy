using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;

namespace ServerCore
{
    public class SendBuffer
    {
        private ConcurrentQueue<ArraySegment<byte>> _sendQueue = new ConcurrentQueue<ArraySegment<byte>>();
        private List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();

        public List<ArraySegment<byte>> PendingList => _pendingList;

        public SendBuffer()
        {
            Clear();
        }

        public void AddSendList(byte[] sendBuffer)
        {
            _sendQueue.Enqueue(sendBuffer);
            while (_sendQueue.Count > 0)
            {
                if (_sendQueue.TryDequeue(out var sendPacket) == true)
                {
                    _pendingList.Add(sendPacket);
                }
            }
        }

        public void OnSend(int sendSize)
        {
            int size = sendSize;
            var sended = new List<ArraySegment<byte>>();
            foreach (var p in _pendingList)
            {
                if (size < p.Count)
                {
                    break;
                }

                sended.Add(p);
                size -= p.Count;
            }

            _pendingList.RemoveAll(p => sended.Contains(p));
        }

        public void Clear()
        {
            _pendingList.Clear();
            _sendQueue.Clear();
        }
    }
}
