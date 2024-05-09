using Game.Main;
using Game.Unary;
using ServerGrpc.Common;
using ServerGrpc.DB.Table;
using ServerGrpc.DB.Worker;
using ServerGrpc.Grpc.Session;

namespace ServerGrpc.Services.Worker
{
    public class CommandExecuter
    {
        private readonly ILogger<CommandExecuter> _logger;

        private readonly DataWorker _dataWorker;

        public CommandExecuter(
            ILogger<CommandExecuter> logger,
            DataWorker dataWorker)
        {
            _logger = logger;
            _dataWorker = dataWorker;
        }

        public async Task<UnaryData> CommandExecute(GameSession session, UnaryData data)
        {
            var res = new UnaryData();
            switch (data.Packet)
            {
                case Game.Types.UnaryDataType.CreateCharacterReq:
                    {
                        throw ErrorHandler.Error(Game.Types.ResultCode.InvalidReqParam, "error test");
                        //res.Packet = Game.Types.CommandPacket.CreateCharacterRes;
                        //res.CreateCharRes = await Command_CreateCharacter(session, data);
                    }
                    break;
                default:
                    break;
            }
            return res;
        }

        private async Task<Unary_CreateCharacterRes> Command_CreateCharacter(GameSession session, UnaryData data)
        {
            var charNo = data.CreateCharReq.CharacterNo;
            var job = data.CreateCharReq.Job;
            var name = data.CreateCharReq.NickName;

            if (charNo <= 0 || charNo > ServerConst.CHARACTER_COUNT_MAX)
            {
                throw ErrorHandler.Error(Game.Types.ResultCode.InvalidReqParam, $"invalid Character no : {charNo}");
            }

            if (job <= Game.Types.JobType.None || job >= Game.Types.JobType.Max)
            {
                throw ErrorHandler.Error(Game.Types.ResultCode.InvalidReqParam, $"invalid job : {job}");
            }

            if (string.IsNullOrEmpty(name) || name.Length > ServerConst.NAME_LENGTH)
            {
                throw ErrorHandler.Error(Game.Types.ResultCode.InvalidReqParam, $"invalid name length : {name}");
            }

            var now = DateTime.UtcNow;
            var characterDb = CharacterDB.Create(session.MberNo, (byte)charNo, job.ToString(), name, ServerConst.START_LEVEL, ServerConst.START_EXP, now, now);
            
            await _dataWorker.CreateCharacter(characterDb);
            var res = new Unary_CreateCharacterRes
            {
                Code = Game.Types.ResultCode.Success,
                Created = characterDb.ToProto(),
                Msg = "Character created"
            };
            return res;
        }
    }
}
