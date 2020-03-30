using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using UdpLink.Server.Command;
using UdpLink.Server.Config;
using UdpLink.Shared.Helpers;

namespace UdpLink.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var baseDir = ProgramUtils.GetBaseDir();
            Directory.SetCurrentDirectory(baseDir);

            var config = new ConfigurationBuilder()
                    .SetBasePath(ProgramUtils.GetBaseDir())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

            ConfigureLogging(config);

            var listenerConfig = config.GetSection("ListenerConfig").Get<ListenerConfig>();
            CreateHostBuilder(args, listenerConfig).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args, ListenerConfig config)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            return Host.CreateDefaultBuilder(args)
                 .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<ListenerConfig>((s) => config);
                    services.AddHostedService<UdpListenerService>();
                    RegisterCmdHandlers(services);

                }).ConfigureLogging((logging) =>
                {
                    logging.ClearProviders();
                    logging.AddNLog();
                });
        }

        private static void RegisterCmdHandlers(IServiceCollection services)
        {
            services.AddTransient<CommandExecutorService>();
            services.AddTransient<ICommandHandler, EchoCommandHandler>();
            services.AddTransient<ICommandHandler, PowershellCommandHandler>();
            services.AddTransient<ICommandHandler, RebootCommandHandler>();
        }

        private static void ConfigureLogging(IConfigurationRoot config)
        {
            var path = config.GetSection("Logging:LogPath").Value;
            if (path == null)
                throw new Exception("Missing configuration section 'Logging:LogPath', ensure the entry is in appsettings.json file");
            GlobalDiagnosticsContext.Set("LogPath", path);
        }
    }
}