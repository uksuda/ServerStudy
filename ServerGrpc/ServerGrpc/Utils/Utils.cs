using System.Text;

namespace ServerGrpc.Utils
{
    public static class CommonManager
    {
        private const string _stringSet = "1234567890abcdefghijklmnopqrstuvwxyz";

        private static readonly Random _random = new(Guid.NewGuid().GetHashCode());

        public static string GetRandomKey()
        {
            return Guid.NewGuid().ToString("N");
        }

        public static int GetRandom(int min, int max)
        {
            return _random.Next(min, max);
        }

        public static string GetRandomString(int length = 0)
        {
            if (length == 0)
            {
                return "";
            }

            var r = new StringBuilder(length);
            // var rr = string.Empty;

            for (int i = 0; i < length; i++)
            {
                int index = _random.Next(0, _stringSet.Length);
                r.Append(_stringSet.ElementAt(index));
                // rr = string.Join(rr, _stringSet.ElementAt(index));
            }
            return r.ToString();
        }
    }
}
