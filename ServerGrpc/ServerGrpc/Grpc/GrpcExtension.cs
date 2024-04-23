using Game.Types;
using Grpc.Core;
using ServerGrpc.Grpc.Session;

namespace ServerGrpc.Grpc
{
    public static class GrpcExtension
    {
        public const string CLIENT_SESSION = "client_session";
        public const string XTID = "xtid";
        public const string CONTEXT_ERROR = "session_error";

        #region Http Context
        public static void SetXtid(this HttpContext ctx, string xtid)
        {
            ctx.Items.Add(XTID, xtid);
        }

        public static string GetXtid(this HttpContext ctx)
        {
            if (ctx.Items.TryGetValue(XTID, out var xtid) == true)
            {
                return xtid.ToString();
            }
            return string.Empty;
        }

        public static void SetClientSession(this HttpContext ctx, ClientSession session)
        {
            ctx.Items.Add(CLIENT_SESSION, session);
        }

        public static ClientSession GetClientSession(this HttpContext ctx)
        {
            if (ctx.Items.TryGetValue(CLIENT_SESSION, out var session) == true)
            {
                return session as ClientSession;
            }
            return null;
        }

        public static void SetContextErr(this HttpContext ctx, ResultCode code)
        {
            ctx.Items.Add(CONTEXT_ERROR, code);
        }

        public static ResultCode GetContextErr(this HttpContext ctx)
        {
            if (ctx.Items.TryGetValue(CONTEXT_ERROR, out var code) == true)
            {
                return (ResultCode)code;
            }
            return ResultCode.Success;
        }
        #endregion

        #region ServerCall Context
        public static void SetXtid(this ServerCallContext context, string xtid)
        {
            var httpCtx = context.GetHttpContext();
            if (httpCtx == null)
            {
                throw new NullReferenceException("http context is empty");
            }
            httpCtx.SetXtid(xtid);
        }

        public static string GetXtid(this ServerCallContext context)
        {
            var httpCtx = context.GetHttpContext();
            if (httpCtx == null)
            {
                throw new NullReferenceException("http context is empty");
            }
            return httpCtx.GetXtid();
        }

        public static ClientSession GetClientSession(this ServerCallContext context)
        {
            var httpCtx = context.GetHttpContext();
            if (httpCtx == null)
            {
                throw new NullReferenceException("http context is empty");
            }
            return httpCtx.GetClientSession();
        }

        //
        public static void SetClientSession(this ServerCallContext context, ClientSession session)
        {
            var httpCtx = context.GetHttpContext();
            if (httpCtx == null)
            {
                throw new NullReferenceException("http context is empty");
            }
            httpCtx.SetClientSession(session);
        }

        //
        public static void SetContextError(this ServerCallContext context, ResultCode code)
        {
            var httpCtx = context.GetHttpContext();
            if (httpCtx == null)
            {
                throw new NullReferenceException("http context is empty");
            }
            httpCtx.Items.Add(CONTEXT_ERROR, code);
        }

        public static (ResultCode, string) GetContextError(this ServerCallContext context)
        {
            var httpCtx = context.GetHttpContext();
            if (httpCtx == null)
            {
                throw new NullReferenceException("http context is empty");
            }
            if (httpCtx.Items.TryGetValue(CONTEXT_ERROR, out var code) == false)
            {
                return (ResultCode.Success, string.Empty);
            }

            var xtid = GetXtid(context);
            return ((ResultCode)code, xtid);
        }

        public static string GetConnectionId(this ServerCallContext context)
        {
            return context.GetHttpContext().Connection.Id;
        }

        public static Microsoft.AspNetCore.Http.Endpoint GetEndPoint(this ServerCallContext context)
        {
            return context.GetHttpContext().GetEndpoint();
        }
        #endregion
    }
}
