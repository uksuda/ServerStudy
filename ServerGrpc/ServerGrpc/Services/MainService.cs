using Game.Main;
using Game.Unary;
using ServerGrpc.Common;
using ServerGrpc.DB.Table;
using ServerGrpc.DB.Worker;
using ServerGrpc.Grpc.Session;
using ServerGrpc.Infra;
using System.ComponentModel;
using System.Reflection;

namespace ServerGrpc.Services
{
    public class MainService
    {
        private readonly ILogger<MainService> _logger;
        private readonly JwtTokenBuilder _tokenBuilder;
        private readonly DataWorker _dataWorker;
        public MainService(
            ILogger<MainService> logger,
            JwtTokenBuilder tokenBuilder,
            DataWorker dataWorker)
        {
            _logger = logger;
            _tokenBuilder = tokenBuilder;
            _dataWorker = dataWorker;
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


            return default;
        }
        #endregion

        #region Unary Data
        public async Task<UnaryData> UnaryDataSend(UnaryData requset, ClientSession session)
        {
            if (requset != null)
            {
                _logger.LogDebug($"{MethodBase.GetCurrentMethod()} - {requset}");
            }

            UnaryData response = null;
            switch (requset.Type)
            {
                case Game.Types.UnaryDataType.CommandReq:
                    response = await UnaryDispatch_CommandReq(requset.CommandReq, session);
                    break;
                default:
                    throw new InvalidEnumArgumentException($"invalid request type {requset.Type}");
            }

            return response;
        }

        private async Task<UnaryData> UnaryDispatch_CommandReq(Unary_CommandReq request, ClientSession session)
        {
            return default;
        }
        #endregion

        #region StreamData
        public bool StreamDispatch(StreamMsg data, ClientStream client, ClientManager manager)
        {
            switch (data.Packet)
            {
                case Game.Types.StreamPacket.Disconnected:
                    break;
                case Game.Types.StreamPacket.MessageSend:
                    break;
                default:
                    throw new InvalidEnumArgumentException($"invalid stream data {data.Packet}");
            }

            return true;
        }
        #endregion
    }
}