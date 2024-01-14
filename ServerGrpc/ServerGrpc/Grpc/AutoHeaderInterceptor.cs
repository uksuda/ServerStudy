using Grpc.Core;
using Grpc.Core.Interceptors;

namespace ServerGrpc.Grpc
{
    public class AutoHeaderInterceptor : Interceptor
    {
        private readonly ILogger<AutoHeaderInterceptor> _logger;
        public AutoHeaderInterceptor(ILogger<AutoHeaderInterceptor> logger)
        {
            _logger = logger;
        }

        private void AutoHead(ServerCallContext context)
        {
            var xtid = context.GetXtid();
            if (string.IsNullOrEmpty(xtid) == true)
            {
                var guid = Guid.NewGuid().ToString("N");
                context.SetXtid(guid);
            }
        }

        public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            AutoHead(context);
            return await continuation(request, context);
        }

        public override Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, ServerCallContext context, ClientStreamingServerMethod<TRequest, TResponse> continuation)
        {
            AutoHead(context);
            return continuation(requestStream, context);
        }

        public override Task ServerStreamingServerHandler<TRequest, TResponse>(TRequest request, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, ServerStreamingServerMethod<TRequest, TResponse> continuation)
        {
            AutoHead(context);
            return continuation(request, responseStream, context);
        }

        public override Task DuplexStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, DuplexStreamingServerMethod<TRequest, TResponse> continuation)
        {
            AutoHead(context);
            return continuation(requestStream, responseStream, context);
        }
    }
}
