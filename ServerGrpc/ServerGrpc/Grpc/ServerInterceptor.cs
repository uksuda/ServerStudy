using Grpc.Core.Interceptors;
using Grpc.Core;
using ServerGrpc.Common;
using System.Diagnostics;

namespace ServerGrpc.Grpc
{
    public class ServerInterceptor : Interceptor
    {
        private readonly ILogger<ServerInterceptor> _logger;

        public ServerInterceptor(ILogger<ServerInterceptor> logger)
        {
            _logger = logger;
        }

        private string MakePreMsg(ServerCallContext context)
        {
            if (context == null)
            {
                return string.Empty;
            }

            var method = context.Method;
            var session = context.GetClientSession();

            string xtid = string.Empty;

            if (session == null)
            {
                xtid = context.GetXtid();
                return $"[Xtid]: {xtid} [Method]: {method} - ";
            }

            var mber = session.MBER_NO;
            return $"[Xtid]: {xtid} [Method]: {method} [Mber]: {mber} - ";
        }

        private void LogMessage(ServerCallContext context, string msg, LogLevel level)
        {
            var preMsg = MakePreMsg(context);
            _logger.Log(level, preMsg + msg);
        }

        private void LogError(ServerCallContext context, Exception e)
        {
            var preMsg = MakePreMsg(context);
            _logger.LogError(preMsg + $"err {e} - {e.Message}");
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

        #region Server's handler
        public override Task DuplexStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, DuplexStreamingServerMethod<TRequest, TResponse> continuation)
        {
            LogCall<TRequest, TResponse>(MethodType.DuplexStreaming, context);

            try
            {
                LogMessage(context, "Begin. ", LogLevel.Debug);
                var res = base.DuplexStreamingServerHandler(requestStream, responseStream, context, continuation);
                LogMessage(context, "End. ", LogLevel.Debug);

                return res;
            }
            catch (Exception e)
            {
                LogError(context, e);
                throw;
            }
        }

        public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            LogCall<TRequest, TResponse>(MethodType.Unary, context);
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Task<TResponse> response = null;
            try
            {
                _logger.LogDebug($"request: {request}");

                LogMessage(context, "Begin. ", LogLevel.Debug);
                response = continuation(request, context);
                LogMessage(context, "End. ", LogLevel.Debug);

                return response;
            }
            catch (ServerException e)
            {
                LogError(context, e);
                var res = ErrorHandler.ErrorResponse<TResponse>(e.Code, e.Msg);
                response = Task.FromResult(res);
                return response;
            }
            catch (Exception e)
            {
                LogError(context, e);
                throw;
            }
            finally
            {
                sw.Stop();
                var time = sw.ElapsedMilliseconds;
                LogMessage(context, $"[Elapsed]={time}ms", LogLevel.Debug);
                _logger.LogDebug($"response: {response.Result}");
            }
        }

        public override Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(IAsyncStreamReader<TRequest> requestStream, ServerCallContext context, ClientStreamingServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                LogCall<TRequest, TResponse>(MethodType.ClientStreaming, context);

                LogMessage(context, "Begin. ", LogLevel.Debug);
                var res = base.ClientStreamingServerHandler(requestStream, context, continuation);
                LogMessage(context, "End. ", LogLevel.Debug);

                return res;
            }
            catch (Exception e)
            {
                LogError(context, e);
                throw;
            }
        }

        public override Task ServerStreamingServerHandler<TRequest, TResponse>(TRequest request, IServerStreamWriter<TResponse> responseStream, ServerCallContext context, ServerStreamingServerMethod<TRequest, TResponse> continuation)
        {
            try
            {
                LogCall<TRequest, TResponse>(MethodType.ServerStreaming, context);

                LogMessage(context, "Begin. ", LogLevel.Debug);
                var res = base.ServerStreamingServerHandler(request, responseStream, context, continuation);
                LogMessage(context, "End. ", LogLevel.Debug);

                return res;
            }
            catch (Exception e)
            {
                LogError(context, e);
                throw;
            }
        }
        #endregion
    }
}