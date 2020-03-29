using System;
using System.Collections.Generic;
using System.Text;
using UdpLink.Shared.Command;

namespace UdpLink.Client.Command
{
    public class PowershellCmdBuilder : ICmdBuilder
    {
        public string CommandKey => "ps";

        public CommandBase BuildCommand(string cmd)
        {
            return new PowershellCommand() { CommandText = cmd };
        }
    }
}
