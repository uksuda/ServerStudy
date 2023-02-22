using Google.Protobuf;
using System.IO;

namespace Network
{
    public static class Utility
    {
        public static byte[] ToByteArray<T>(this T o) where T : IMessage
        {
            if (o == null)
                return null;

            using MemoryStream ms = new MemoryStream();
            o.WriteTo(ms);
            return ms.ToArray();
        }

        public static T ToObject<T>(this byte[] buf) where T : IMessage<T>, new()
        {
            if (buf == null)
                return default(T);

            using MemoryStream ms = new MemoryStream();
            ms.Write(buf, 0, buf.Length);
            ms.Seek(0, SeekOrigin.Begin);

            MessageParser<T> parser = new MessageParser<T>(() => new T());
            return parser.ParseFrom(ms);
        }
    }
}
