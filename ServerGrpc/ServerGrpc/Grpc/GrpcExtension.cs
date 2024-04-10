﻿using Grpc.Core;
using ServerGrpc.Common;

namespace ServerGrpc.Grpc
{
    public static class GrpcExtension
    {
        public const string CLIENT_SESSION = "client_session";
        public const string XTID = "xtid";

        public static void SetXtid(this ServerCallContext context, string xtid)
        {
            context.RequestHeaders.Add(XTID, xtid);
        }

        public static string GetXtid(this ServerCallContext context)
        {
            return context.RequestHeaders.GetValue(XTID);
        }

        public static ClientSession GetClientSession(this ServerCallContext context)
        {
            var httpContext = context.GetHttpContext();
            if (httpContext == null)
            {
                throw new NullReferenceException("http context is empty");
            }

            httpContext.Items.TryGetValue(CLIENT_SESSION, out var session);
            return session as ClientSession;
        }

        public static void SetClientSession(this ServerCallContext context, ClientSession session)
        {
            var httpContext = context.GetHttpContext();
            if (httpContext == null)
            {
                throw new NullReferenceException("http context is empty");
            }

            httpContext.Items.Add(CLIENT_SESSION, session);
        }

        public static string GetConnectionId(this ServerCallContext context)
        {
            return context.GetHttpContext().Connection.Id;
        }

        public static Microsoft.AspNetCore.Http.Endpoint GetEndPoint(this ServerCallContext context)
        {
            return context.GetHttpContext().GetEndpoint();
        }
    }
}
