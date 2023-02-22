using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
    public class SessionManager
    {
        #region SINGLETON
        private static SessionManager _instance = null;
        public static SessionManager Get()
        {
            if (_instance == null)
            {
                _instance = new SessionManager();
            }

            return _instance;
        }
        #endregion

        private int _generatedIndex = 0;
        private object _lock = new object();

        private readonly ConcurrentDictionary<int, Session> _sessions = new ConcurrentDictionary<int, Session>();

        public bool Initialize()
        {
            return true;
        }

        public Session GetSession(int index)
        {
            _sessions.TryGetValue(index, out var session);
            return session;
        }

        public bool AddSession(Session session)
        {
            return _sessions.TryAdd(session.Index, session);
        }

        public int GenerateSessionIndex()
        {
            return Interlocked.Increment(ref _generatedIndex);
        }

        public async Task StartSessionsRecv()
        {
            while (true)
            {
                foreach (var session in _sessions.Values)
                {
                    if (session == null || session.Disconnected == true)
                    {
                        continue;
                    }

                    await Task.Run(async () =>
                    {
                        await session.OnRecv();
                    });
                }
            }
        }
    }
}
