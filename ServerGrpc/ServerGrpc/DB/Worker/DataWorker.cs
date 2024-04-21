using Dapper;
using ServerGrpc.Common;
using ServerGrpc.DB.Table;
using ServerGrpc.Utils;
using StackExchange.Redis;

namespace ServerGrpc.DB.Worker
{
    public class DataWorker
    {
        private readonly ILogger<DataWorker> _logger;

        private readonly DBSelector _selector;
        private readonly DBUpdater _updater;

        private readonly AccountRedisContext _accountRedis;
        private readonly GameRedisContext _gameRedis;

        private readonly AccountDBContext _accountDbCtx;
        private readonly GameDBContext _gameDbCtx;

        public DataWorker(
            ILogger<DataWorker> logger, 
            DBSelector selector, 
            DBUpdater updater, 
            AccountRedisContext accountRedis, 
            GameRedisContext gameRedis,
            AccountDBContext accountDbCtx,
            GameDBContext gameDbCtx)
        {
            _logger = logger;
            _selector = selector;
            _updater = updater;
            _accountRedis = accountRedis;
            _gameRedis = gameRedis;
            _accountDbCtx = accountDbCtx;
            _gameDbCtx = gameDbCtx;
        }

        #region Direct & Specific
        public async Task<int> CreateMember(MemberDB db)
        {
            var mberNo = await CreateMemberDB(db);
            db.mber_no = mberNo;

            await SetMemberToCache(db);
            return mberNo;
        }

        private async Task<int> CreateMemberDB(MemberDB db)
        {
            using var dbCtx = _accountDbCtx.GetDbContext();
            using var tran = await dbCtx.BeginTransactionAsync();

            try
            {
                var (query, param) = MemberDB.Insert(db);
                var selectQuery = "select last_insert_id()";
                await dbCtx.ExecuteAsync(query, param, transaction: tran);
                var mberNo = await dbCtx.QueryFirstOrDefaultAsync<int>(selectQuery, transaction: tran);

                await tran.CommitAsync();
                return mberNo;
            }
            catch (Exception e)
            {
                await tran.RollbackAsync();
                throw ErrorHandler.CatchThrow(e, "Create member fail");
            }
        }

        public async ValueTask CreateCharacter(CharacterDB db)
        {
            try
            {
                var (query, param) = CharacterDB.Insert(db);
                using var dbCtx = _gameDbCtx.GetDbContext();
                await dbCtx.ExecuteAsync(query, param);

                await SetCharacterToCache(db);
            }
            catch (Exception e)
            {
                throw ErrorHandler.CatchThrow(e, "CreateCharacter fail");
            }
        }
        #endregion

        #region MemberDB
        private async ValueTask SetMemberToCache(MemberDB db)
        {
            var key = CommonManager.GetRedisKey(MemberDB.Table, db.mber_no);
            var val = CommonManager.Serialize(db);
            await _accountRedis.Cache().StringSetAsync(key, val, MemberDB.Expire);
        }

        public async Task<MemberDB> GetMember(int mberNo)
        {
            var key = CommonManager.GetRedisKey(MemberDB.Table, mberNo);
            var r = await _accountRedis.Cache().StringGetAsync(key);
            if (r.HasValue == true)
            {
                return CommonManager.Deserialize<MemberDB>(r);
            }

            var (query, param) = MemberDB.Select(mberNo);
            var select = new DBSelectInfo(DataBaseType.Account, query, param);
            await _selector.Write(select);

            var temp = await select.CallAsync();
            var mberDB = temp.Cast<MemberDB>().ToList().FirstOrDefault();
            if (mberDB != null)
            {
                await SetMemberToCache(mberDB);
            }
            return mberDB;
        }

        public async Task<MemberDB> GetMemberById(string mberId)
        {
            // not in cache
            var (query, param) = MemberDB.Select(mberId);
            var select = new DBSelectInfo(DataBaseType.Account, query, param);
            await _selector.Write(select);

            var temp = await select.CallAsync();
            var mberDB = temp.Cast<MemberDB>().ToList().FirstOrDefault();
            if (mberDB != null)
            {
                await SetMemberToCache(mberDB);
            }
            return mberDB;
        }

        public async ValueTask UpdateMember(MemberDB db)
        {
            await SetMemberToCache(db);

            var (query, param) = MemberDB.Update(db.mber_no, db.last_login_time);
            var update = new DBUpdateInfo(DataBaseType.Account, query, param, MemberDB.Table);
            await _updater.Write(update);
        }
        #endregion

        #region CharacterDB
        private async ValueTask SetCharacterToCache(CharacterDB db)
        {
            var key = CommonManager.GetRedisKey(CharacterDB.Table, db.mber_no);
            var val = CommonManager.Serialize(db);

            var batch = _gameRedis.Cache().CreateBatch();
            _ = batch.HashSetAsync(key, (int)db.character_no, val);
            _ = batch.KeyExpireAsync(key, CharacterDB.Expire);
            batch.Execute();

            await Task.CompletedTask;
        }

        private async ValueTask SetCharacterToCache(int mberNo, IEnumerable<CharacterDB> dbs)
        {
            if (dbs == null || dbs.Any() == false)
            {
                return;
            }

            var key = CommonManager.GetRedisKey(CharacterDB.Table, mberNo);
            var hashEntrys = new List<HashEntry>();
            foreach (var db in dbs)
            {
                var val = CommonManager.Serialize(db);
                var entry = new HashEntry((int)db.character_no, val);
                hashEntrys.Add(entry);
            }

            var batch = _gameRedis.Cache().CreateBatch();
            _ = batch.HashSetAsync(key, hashEntrys.ToArray());
            _ = batch.KeyExpireAsync(key, CharacterDB.Expire);
            batch.Execute();

            await Task.CompletedTask;
        }

        public async Task<IEnumerable<CharacterDB>> GetCharacterDBList(int mberNo)
        {
            var result = new List<CharacterDB>();
            var key = CommonManager.GetRedisKey(CharacterDB.Table, mberNo);
            var r = await _gameRedis.Cache().HashGetAllAsync(key);
            if (r.Length > 0)
            {
                foreach (var h in r)
                {
                    var val = CommonManager.Deserialize<CharacterDB>(h.Value);
                    result.Add(val);
                }
                return result;
            }

            var (query, param) = CharacterDB.Select(mberNo);
            var select = new DBSelectInfo(DataBaseType.Game, query, param);
            await _selector.Write(select);

            var temp = await select.CallAsync();
            var characterDbs = temp.Cast<CharacterDB>().ToList();
            if (characterDbs != null && characterDbs.Any() == true)
            {
                await SetCharacterToCache(mberNo, characterDbs);
            }
            return characterDbs;
        }

        public async ValueTask UpdateCharacterDB(CharacterDB db)
        {
            await SetCharacterToCache(db);

            var (query, param) = CharacterDB.Update(db);
            var update = new DBUpdateInfo(DataBaseType.Game, query, param, CharacterDB.Table);
            await _updater.Write(update);
        }
        #endregion
    }
}
