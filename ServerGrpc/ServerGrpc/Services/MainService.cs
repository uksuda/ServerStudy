using Game.Main;
using Game.Unary;
using ServerGrpc.Common;
using ServerGrpc.DB.Table;
using ServerGrpc.DB.Worker;
using ServerGrpc.Grpc.Session;
using ServerGrpc.Infra;
using ServerGrpc.Services.Worker;
using System.ComponentModel;
using System.Reflection;

namespace ServerGrpc.Services
{
    public class MainService
    {
        private readonly ILogger<MainService> _logger;
        private readonly JwtTokenBuilder _tokenBuilder;
        private readonly DataWorker _dataWorker;

        private readonly CommandExecuter _commandExecuter;
        private readonly StreamDispatcher _streamDispatcher;

        public MainService(
            ILogger<MainService> logger,
            JwtTokenBuilder tokenBuilder,
            DataWorker dataWorker,
            CommandExecuter commandExecuter,
            StreamDispatcher streamDispatcher)
        {
            _logger = logger;
            _tokenBuilder = tokenBuilder;
            _dataWorker = dataWorker;
            _commandExecuter = commandExecuter;
            _streamDispatcher = streamDispatcher;
        }

        #region Join & Login
        public async Task<JoinRes> Join(JoinReq requset, string xtid)
        {
            var mberDb = await _dataWorker.GetMemberById(requset.Id);
            if (mberDb != null)
            {
                throw ErrorHandler.Error(Game.Types.ResultCode.Duplicated, "member id duplicated");
            }

            var now = DateTime.UtcNow;
            mberDb = MemberDB.Create(0, requset.Id, requset.Password, now, now);
            var mberNo = await _dataWorker.CreateMember(mberDb);
            mberDb.mber_no = mberNo;

            var token = _tokenBuilder.GenerateToken(xtid, mberNo);

            var res = new JoinRes
            {
                Result = new Game.Common.Result
                {
                    Code = Game.Types.ResultCode.Success
                },
                Token = token,
            };
            return res;
        }

        public async Task<LoginRes> Login(LoginReq requset, string xtid)
        {
            var mberDb = await _dataWorker.GetMemberById(requset.Id);
            if (mberDb == null || requset.Password.Equals(mberDb.password) == false)
            {
                throw ErrorHandler.Error(Game.Types.ResultCode.NotExist, $"not exist member. id {requset.Id}");
            }

            var characterList = await _dataWorker.GetCharacterDBList(mberDb.mber_no);
            var token = _tokenBuilder.GenerateToken(xtid, mberDb.mber_no);

            var res = new LoginRes
            {
                Result = new Game.Common.Result
                {
                    Code = Game.Types.ResultCode.Success
                },
                Token = token,
            };
            return res;
        }
        #endregion

        #region Unary Data
        public async Task<UnaryData> UnaryDataSend(UnaryData requset, GameSession session)
        {
            try
            {
                var res = await _commandExecuter.CommandExecute(session, requset);
                return res;
            }
            catch (ServerException e)
            {
                e.Response = ErrorHandler.ErrorResponse_Unary(e, requset.Packet);
                throw;
            }
        }
        #endregion

        #region StreamData
        public async Task<bool> StreamDispatch(ClientSession client)
        {
            await client.ReadAsync((data) =>
            {
                try
                {
                    return _streamDispatcher.StreamDispatch(client, data);
                }
                catch (ServerException e)
                {
                    if (e.Response != null)
                    {
                        var streamMsg = (StreamData)e.Response;
                        _ = Task.Run(async () =>
                        {
                            await client.SendMsg(streamMsg);
                        });
                        _logger.LogError($"StreamDispatch err: {e}");
                    }
                    return true;
                }
            });

            _logger.LogDebug($"StreamDispatch end. {client.Mber}");
            return true;
        }
        #endregion
    }
}