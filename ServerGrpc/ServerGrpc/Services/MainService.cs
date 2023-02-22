using Grpc.Core;
using network.main;
using ServerGrpc.Common;
using System.Reflection;

namespace ServerGrpc.Services
{
    public class MainService
    {
        private readonly ILogger<MainService> _logger;
        public MainService(ILogger<MainService> logger)
        {
            _logger = logger;
            _logger.LogDebug("MainService started");
        }

        public async Task<UnaryData> UnaryDataSend(UnaryData request, ClientSession session)
        {
            if (request != null)
            {
                _logger.LogDebug($"{MethodBase.GetCurrentMethod()} - {request}");
            }

            var response = new UnaryData
            {
                Type = network.types.UnaryDataType.SampleRes,
                SampleRes = new network.unary.Unary_SampleRes
                {
                    Err = 0,
                    S = "sample data"
                },
            };

            return response;
        }

        //public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        //{
        //    return Task.FromResult(new HelloReply
        //    {
        //        Message = "Hello " + request.Name
        //    });
        //}
    }
}