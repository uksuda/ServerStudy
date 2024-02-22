using Google.Protobuf;
using Grpc.Core;
using Network.Common;
using Network.Types;

namespace ServerGrpc.Common
{
    public class ServerException : Exception
    {
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
    }
}
