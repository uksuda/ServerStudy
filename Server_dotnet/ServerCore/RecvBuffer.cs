using Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerCore
{
    public class RecvBuffer
    {
        private byte[] _buffer = null;
        private int _position = 0;

        public RecvBuffer(int buffSize)
        {
            _buffer = new byte[buffSize];
            _position = 0;
        }

        public void OnRecv(int recvSize)
        {
            if (_position + recvSize > _buffer.Length)
            {
                return;
            }

            _position += recvSize;
        }

        public bool GetPacket(out Packet recvPacket)
        {
            var offsetSize = sizeof(int);
            if (_position < offsetSize)
            {
                recvPacket = null;
                return false;
            }

            var sizeBuffer = new ArraySegment<byte>(_buffer, 0, offsetSize);
            var packetSize = BitConverter.ToInt32(sizeBuffer);

            if (_position - offsetSize < packetSize)
            {
                recvPacket = null;
                return false;
            }

            var buffer = new ArraySegment<byte>(_buffer, offsetSize, _position).ToArray();
            _position = (_position > packetSize + offsetSize) ? _position - (packetSize + offsetSize) : 0;
            var p = buffer.ToObject<Packet>();

            // Array.Copy
            Array.Copy(_buffer, packetSize + offsetSize, _buffer, 0, _position);

            recvPacket = p;
            return true;
        }

        public ArraySegment<byte> GetAvailableBuffer()
        {
            if (_buffer.Length <= _position)
            {
                // 
                return null;
            }

            return new ArraySegment<byte>(_buffer, _position, _buffer.Length - _position);
        }

        public void Clear()
        {
            //_buffer.Initialize();
            Array.Clear(_buffer, 0, _buffer.Length);
            _position = 0;
        }
    }
}
