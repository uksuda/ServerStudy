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
        public static ServerException Error(ResultCode code, string msg)
        {
            return new ServerException(code, msg);
        }

        public static ServerException CatchThrow(Exception e, string msg)
        {
            return new ServerException(ResultCode.ServerInternalError, e, msg);
        }

        public static TResponse CreateResponse<TResponse>(ServerCallContext context, ServerException e)
        {
            var result = new Result
            {
                Code = e.Code,
                Msg = e.Msg,
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
    }
}
