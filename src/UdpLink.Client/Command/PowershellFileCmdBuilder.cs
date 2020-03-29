using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UdpLink.Shared.Command;

namespace UdpLink.Client.Command
{
    public class PowershellFileCmdBuilder : ICmdBuilder
    {
        public string CommandKey => "psf";

        public CommandBase BuildCommand(string cmd)
        {
            var rawCmd = File.ReadAllText(cmd);
            return new PowershellCommand() { CommandText = rawCmd };
        }
    }
}
