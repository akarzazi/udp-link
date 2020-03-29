using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UdpLink.Server.Command;
using UdpLink.Server.Config;
using UdpLink.Shared.Helpers;

namespace UdpLink.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                    .SetBasePath(ProgramUtils.GetBaseDir())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();
            var listenerConfig = config.GetSection("ListenerConfig").Get<ListenerConfig>();

            CreateHostBuilder(args, listenerConfig).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args, ListenerConfig config)
        {
            return Host.CreateDefaultBuilder(args)
                 .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<ListenerConfig>((s) => config);                
                    services.AddHostedService<UdpListenerService>();
                    RegisterCmdHandlers(services);

                });

        }

        private static void RegisterCmdHandlers(IServiceCollection services)
        {
            services.AddTransient<CommandExecutorService>();
            services.AddTransient<EchoCommandHandler>();
            services.AddTransient<PowershellCommandHandler>();
            services.AddTransient<RebootCommandHandler>(); 
            services.AddTransient<ICommandHandler, EchoCommandHandler>();
            services.AddTransient<ICommandHandler, PowershellCommandHandler>();
            services.AddTransient<ICommandHandler, RebootCommandHandler>();
        }
    }
}