using System.Text;
using System.Text.Json;

namespace ServerGrpc.Utils
{
    public static class CommonManager
    {
        private const string _stringSet = "1234567890abcdefghijklmnopqrstuvwxyz";

        private static readonly Random _random = new(Guid.NewGuid().GetHashCode());

        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            IncludeFields = true,
            PropertyNameCaseInsensitive = true,
        };

        #region Random
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
        #endregion

        #region Json
        public static string Serialize<T>(T obj)
        {
            return JsonSerializer.Serialize(obj, _jsonOptions);
        }

        public static T Deserialize<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json, _jsonOptions);
        }
        #endregion

        #region Redis
        public static string GetRedisKey(string key, int val)
        {
            return $"{key}:{val}";
        }

        public static string GetRedisKey(string key, int val, int val2)
        {
            return $"{key}:{val}:{val2}";
        }
        #endregion
    }
}
