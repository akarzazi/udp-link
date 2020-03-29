using System.Diagnostics;
using System.Drawing;
using System.IO;

using Microsoft.Extensions.Configuration;
using UdpLink.Client.Config;
using UdpLink.Shared.Helpers;
using Console = Colorful.Console;

namespace UdpLink.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                            .SetBasePath(ProgramUtils.GetBaseDir())
                            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                            .Build();
            var endpoint = config.GetSection("EndPointConfig").Get<EndPointConfig>();

            try
            {
                new UdpClientService(endpoint).Start();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Fatal Error: " + ex, Color.Red);
                throw;
            }

        }
    }
}