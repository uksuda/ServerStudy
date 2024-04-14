using MySqlConnector;
using ServerGrpc.Common;

namespace ServerGrpc.DB
{
    public class DBConnection
    {
        private readonly string _connectStr;
        private readonly MySqlConnection _connection;

        public DBConnection(string connectStr)
        {
            _connectStr = connectStr;
            _connection = new MySqlConnection(_connectStr);
        }

        public MySqlConnection GetConnection()
        {
            var connection = _connection.Clone();
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }
            return connection;
        }
    }

    public abstract class DBContext
    {
        private readonly Dictionary<DataBaseRWType, DBConnection> _dbMap = new Dictionary<DataBaseRWType, DBConnection>();

        public DBContext(AppSettings appsetting, DataBaseType database)
        {
            if (appsetting.Database == null)
            {
                throw new ArgumentNullException("appsettings database is empty");
            }

            if (appsetting.Database.TryGetValue(database, out DatabaseSection value) == false)
            {
                throw new ArgumentNullException($"invalid database type : {database}");
            }

            var connectStr = value.GetConnectionString();
            var readConnect = new DBConnection(connectStr);
            var writeConnect = new DBConnection(connectStr);

            _dbMap.Add(DataBaseRWType.Read, readConnect);
            _dbMap.Add(DataBaseRWType.ReadWrite, writeConnect);
        }

        public MySqlConnection GetDbContext(DataBaseRWType rwType = DataBaseRWType.ReadWrite)
        {
            if (_dbMap.TryGetValue(rwType, out DBConnection value) == false || value == null)
            {
                throw new ArgumentException($"invalid database rw : {rwType}");
            }
            return value.GetConnection();
        }
    }


    public class AccountDBContext : DBContext
    {
        public AccountDBContext(AppSettings appsettins) : base(appsettins, DataBaseType.Account)
        {
        }
    }

    public class GameDBContext : DBContext
    {
        public GameDBContext(AppSettings appsettins) : base(appsettins, DataBaseType.Game)
        {
        }
    }

    public class LogDBContext : DBContext
    {
        public LogDBContext(AppSettings appsettins) : base(appsettins, DataBaseType.Log)
        {
        }
    }
}
