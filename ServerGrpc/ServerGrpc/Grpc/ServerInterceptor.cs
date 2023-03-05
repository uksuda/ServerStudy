using Grpc.Core.Interceptors;
using Grpc.Core;
using ServerGrpc.Common;

namespace ServerGrpc.Grpc
{
    public class ServerInterceptor : Interceptor
    {
        private readonly ILogger<ServerInterceptor> _logger;

        public ServerInterceptor(ILogger<ServerInterceptor> logger)
        {
            _logger = logger;
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                var session = context.GetClientSession();
                if (session == null)
                {
                    var xtid = Guid.NewGuid().ToString();
                    var id = context.GetHttpContext().Request.Headers["id"];
                    var password = context.GetHttpContext().Request.Headers["password"];

                    session = new ClientSession(xtid, id, password);
                    context.SetClientSession(session);
                }

                _logger.LogDebug($"call Type: {MethodType.Unary}. Method: {context.Method}. {session.XTID} id: {session.ID}, password: {session.PASS}");
                _logger.LogDebug($"request: {request}");

                var res = await continuation(request, context);
                _logger.LogDebug($"response: {res}");
                return res;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error thrown by {context.Method}.");
                throw;
            }
        }

        public override Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, ServerCallContext context, ClientStreamingServerMethod<TRequest, TResponse> continuation)
        {
            LogCall<TRequest, TResponse>(MethodType.ClientStreaming, context);
            return base.ClientStreamingServerHandler(requestStream, context, continuation);
        }

        public override Task ServerStreamingServerHandler<TRequest, TResponse>(TRequest request, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, ServerStreamingServerMethod<TRequest, TResponse> continuation)
        {
            LogCall<TRequest, TResponse>(MethodType.ServerStreaming, context);
            return base.ServerStreamingServerHandler(request, responseStream, context, continuation);
        }

        public override Task DuplexStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, DuplexStreamingServerMethod<TRequest, TResponse> continuation)
        {
            LogCall<TRequest, TResponse>(MethodType.DuplexStreaming, context);

            try
            {
                var session = context.GetClientSession();
                if (session == null)
                {
                    var xtid = Guid.NewGuid().ToString();
                    var id = context.GetHttpContext().Request.Headers["id"];
                    var password = context.GetHttpContext().Request.Headers["password"];

                    session = new ClientSession(xtid, id, password);
                    context.SetClientSession(session);
                }

                _logger.LogDebug($"call Type: {MethodType.DuplexStreaming}. Method: {context.Method}. {session.XTID} id: {session.ID}, password: {session.PASS}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error thrown by {context.Method}.");
                throw;
            }

            return base.DuplexStreamingServerHandler(requestStream, responseStream, context, continuation);
        }

        private void LogCall<TRequest, TResponse>(MethodType methodType, ServerCallContext context)
            where TRequest : class
            where TResponse : class
        {
            _logger.LogWarning($"Starting call. Type: {methodType}. Request: {typeof(TRequest)}. Response: {typeof(TResponse)}");
            WriteMetadata(context.RequestHeaders, "caller-user");
            WriteMetadata(context.RequestHeaders, "caller-machine");
            WriteMetadata(context.RequestHeaders, "caller-os");

            void WriteMetadata(Metadata headers, string key)
            {
                var headerValue = headers.GetValue(key) ?? "(unknown)";
                _logger.LogWarning($"{key}: {headerValue}");
            }
        }
    }
}
