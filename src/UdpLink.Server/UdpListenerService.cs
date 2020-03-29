using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using UdpLink.Server.Command;
using UdpLink.Server.Config;
using UdpLink.Shared.Command;
using UdpLink.Shared.Helpers;

namespace UdpLink.Server
{
    public class UdpListenerService : BackgroundService
    {
        private readonly ILogger<UdpListenerService> _logger;
        private readonly ListenerConfig _listenerConfig;
        private readonly CommandExecutorService _commandExecutorService;

        public UdpListenerService(ILogger<UdpListenerService> logger, CommandExecutorService commandExecutorService, ListenerConfig listenerConfig)
        {
            _logger = logger;
            _commandExecutorService = commandExecutorService;
            _listenerConfig = listenerConfig;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using UdpClient listener = new UdpClient(_listenerConfig.Port);
            UdpHelper.ConfigureSocket(listener);

            _logger.LogInformation($"Listening on {_listenerConfig.Port}");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Waiting for broadcast");

                    var receiveResult = await listener.ReceiveAsync();
                    IPEndPoint remoteEP = receiveResult.RemoteEndPoint;
                    byte[] bytes = receiveResult.Buffer;

                    _logger.LogInformation($"Received payload from {remoteEP}");

                    var command = TryGetCommand(bytes);
                    if (command == null)
                    {
                        await Reply(listener, remoteEP, "Cannot read payload");
                    }
                    else
                    {
                        var response = _commandExecutorService.ExecuteCommand(command);
                        await Reply(listener, remoteEP, response);
                    }
                }
                catch (SocketException e)
                {
                    _logger.LogError(e, "socket error");
                    throw e;
                }
            }
        }

        private async Task Reply(UdpClient listener, IPEndPoint groupEP, string response)
        {
            byte[] reply = Encoding.UTF8.GetBytes($"Response @{DateTime.Now.ToLongTimeString()} To {groupEP} \n{response}");
            var payloadBytes = PayloadHelper.CreatePayloadBytes(reply, _listenerConfig.Secret);
            await listener.SendAsync(payloadBytes, payloadBytes.Length, groupEP);
        }

        private CommandBase TryGetCommand(byte[] payloadBytes)
        {
            try
            {
                var decryptedBytes = PayloadHelper.DecodePayloadBytes(payloadBytes, _listenerConfig.Secret);
                return CommandEncoder.Decode(decryptedBytes);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Cannot read payload");
                return null;
            }
        }
    }
}
