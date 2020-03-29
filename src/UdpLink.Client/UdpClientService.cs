using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;

using UdpLink.Client.Command;
using UdpLink.Client.Config;
using UdpLink.Shared.Command;
using UdpLink.Shared.Data;
using UdpLink.Shared.Helpers;
using UdpLink.Shared.Security;
using Console = Colorful.Console;

namespace UdpLink.Client
{
    public class UdpClientService
    {
        public const string Helptext = @"
Syntax
command:value

Available commands
echo                    Remote server replies with the same payload.
ps                      Executes powershell command on the remote server.
psf                     Executes a local powershell file on the remote server.
reboot                  Reboots the remote server

Samples
echo:hello              Echo the word 'hello'
ps:ipconfig             Executes ipconfig command
psf:'my file.ps1'       Executes the file content as powershell               
";

        public EndPointConfig RemoteEndPoint { get; private set; }

        public UdpClientService(EndPointConfig remoteEndPoint)
        {
            RemoteEndPoint = remoteEndPoint;
        }

        public void Start()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(RemoteEndPoint.Host), RemoteEndPoint.Port);

            Console.WriteLine($"Binding to endpoint {ep}", Color.White);

            var client = new UdpClient();
            client.Connect(ep);

            commandStart:
            Console.WriteLine(Helptext, Color.LightGray);

            while (true)
            {
                Console.WriteLine("Enter command:", Color.Gray);
                 var cmdLine = Console.ReadLine();
                 //var cmdLine = "echo:a";
                var command = CmdParser.ParseCommandLine(cmdLine);
                if (command == null)
                {
                    Console.WriteLine($"Unknown Command {cmdLine}", Color.Red);
                    goto commandStart;
                }

                var payloadBytes = CreatePayloadBytes(command);
                client.Send(payloadBytes, payloadBytes.Length);

                Console.WriteLine($"Waiting for response ...");

                // receive
                var responsePayloadBytes = client.Receive(ref ep);
                Console.WriteLine($"Received data from {ep}");

                var response = ReadResponse(responsePayloadBytes);
                Console.WriteLine(response, Color.AntiqueWhite);
            }
        }

        private byte[] CreatePayloadBytes(CommandBase command)
        {
            var commandBytes = CommandEncoder.Encode(command);
            return PayloadHelper.CreatePayloadBytes(commandBytes, RemoteEndPoint.Secret);
        }

        private string ReadResponse(byte[] payloadBytes)
        {
            try
            {
                var decryptedBytes = PayloadHelper.DecodePayloadBytes(payloadBytes, RemoteEndPoint.Secret);
                return Encoding.UTF8.GetString(decryptedBytes, 0, decryptedBytes.Length);
            }
            catch (Exception ex)
            {
                Console.Write($"Cannot read response \n{ex}");
                return "";
            }
        }
    }
}
