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
        }

        public async Task<UnaryData> UnaryDataSend(UnaryData request, ClientSession session)
        {
            if (request != null)
            {
                _logger.LogDebug($"{MethodBase.GetCurrentMethod()} - {request}");
            }

            var response = new UnaryData
            {

            };

            return response;
        }
    }
}