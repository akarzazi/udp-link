using System;
using System.Collections.Generic;
using System.Text;
using UdpLink.Shared.Command;

namespace UdpLink.Client.Command
{
    public class EchoCmdBuilder : ICmdBuilder
    {
        public string CommandKey => "echo";

        public CommandBase BuildCommand(string cmd)
        {
            return new EchoCommand() { Text = cmd };
        }
    }
}
