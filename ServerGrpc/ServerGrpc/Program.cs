using ServerGrpc.Logger;
using System.Diagnostics;
using System.Net;
using System.Reflection;

namespace ServerGrpc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ThreadPool.SetMinThreads(Environment.ProcessorCount * 10, Environment.ProcessorCount * 2);

            AppLogManager.Init();
            var logger = AppLogManager.GetLogger<Program>();

            PrintInfomation(logger);

            //app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            //app.Run();

            // Additional configuration is required to successfully run gRPC on macOS.
            // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

            try
            {
                var app = new AppMain(logger, args);
                app.Run();
            }
            catch (Exception e)
            {
                logger.LogError(e.ToString());
            }
        }

        public static void PrintInfomation(ILogger logger)
        {
            var host = Dns.GetHostName();
            var ipEntry = Dns.GetHostEntry(host);

            logger.LogInformation("Name: [{0}]", Assembly.GetEntryAssembly().FullName);
            logger.LogInformation("UserName={0}", Environment.UserName);
            logger.LogInformation("ProcessId={0}", Environment.ProcessId);
            logger.LogInformation("ProcessorCount={0}", Environment.ProcessorCount);
            logger.LogInformation("ThreadCount={0}", Process.GetCurrentProcess().Threads.Count);
            logger.LogInformation("Host: [{0}]", host);

            foreach (var ip in ipEntry.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    logger.LogInformation("Endpoint: {0}", ip);
                }
            }
        }
    }
}