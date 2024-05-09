using Google.Protobuf;
using Grpc.Core;
using Game.Common;
using Game.Types;
using Game.Main;

namespace ServerGrpc.Common
{
    public class ServerException : Exception
    {
        public object Response { get; set; }
        public ResultCode Code { get; private set; }
        public string Msg { get; private set; }

        public ServerException(ResultCode code, string msg = "")
            : base(msg)
        {
            Code = code;
            Msg = msg;
        }

        public ServerException(ResultCode code, Exception e, string msg = "")
            : base(e.Message, e)
        {
            Code = code;
            Msg = msg;
        }
    }

    public static class ErrorHandler
    {
        public const string Meta_ErrorCode = "errorcode";
        public const string Meta_Message = "message";

        private static StatusCode ToStatusCode(ResultCode code)
        {
            if (code == ResultCode.ServerInternalError)
            {
                return StatusCode.Internal;
            }
            return StatusCode.OK;
        }

        private static Status ToRpcStatus(ServerException e)
        {
            var code = ToStatusCode(e.Code);
            var msg = $"{e.GetType().Name}.";
            return new Status(code, msg, e);
        }

        private static Metadata ToRpcMetaData(ServerException e, bool isRelease = false)
        {
            var trailers = new Metadata();
            trailers.Add(Meta_ErrorCode, e.Code.ToString());
            if (isRelease == false)
            {
                trailers.Add(Meta_Message, e.Message);
            }
            return trailers;
        }


        public static RpcException ToRpcException(ServerException e, bool isRelease = false)
        {
            var status = ToRpcStatus(e);
            var trailers = ToRpcMetaData(e, isRelease);
            return new RpcException(status, trailers);
        }

        public static ServerException Error(ResultCode code, string msg)
        {
            return new ServerException(code, msg);
        }

        public static ServerException CatchThrow(Exception e, string msg)
        {
            return new ServerException(ResultCode.ServerInternalError, e, msg);
        }

        public static TResponse ErrorResponse<TResponse>(ResultCode code, string msg = "")
        {
            var result = new Result
            {
                Code = code,
                Msg = msg,
            };

            TResponse res = Activator.CreateInstance<TResponse>();
            var proto = res as IMessage;
            var field = proto.Descriptor.FindFieldByName("result");
            if (field != null)
            {
                field.Accessor.SetValue(proto, result);
            }
            return res;
        }

        public static T CreateResponse<T>(ServerCallContext context, ServerException e, bool isRelease = false)
        {
            var res = Activator.CreateInstance<T>();
            context.ResponseTrailers.Add(Meta_ErrorCode, e.Code.ToString());
            if (isRelease == false)
            {
                context.ResponseTrailers.Add(Meta_Message, e.Message);
            }
            return res;
        }

        public static UnaryData ErrorResponse_Unary(ServerException e, Game.Types.UnaryDataType reqPacket)
        {
            var res = new UnaryData();
            switch (reqPacket)
            {
                case UnaryDataType.CreateCharacterReq:
                    {
                        res.Packet = UnaryDataType.CreateCharacterRes;
                        res.CreateCharRes = new Game.Unary.Unary_CreateCharacterRes
                        {
                            Code = e.Code,
                            Msg = e.Msg,
                        };
                    }
                    break;
                default:
                    break;
            }
            return res;
        }

        public static ServerException Error_Stream(ResultCode code, string msg, Game.Types.StreamPacket reqPacket)
        {
            var ex = new ServerException(code, msg);
            var res = new StreamData();
            switch (reqPacket)
            {
                case StreamPacket.ConnectReq:
                    {
                        res.Packet = StreamPacket.ConnectRes;
                        res.ConnectRes = new Game.Stream.Stream_ConnectRes
                        {
                            Code = code,
                            Msg = msg,
                        };
                    }
                    break;
                default:
                    break;
            }
            ex.Response = res;
            return ex;
        }
    }
}
