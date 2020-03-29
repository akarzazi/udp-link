using System;
using System.Collections.Generic;
using System.Text;
using UdpLink.Shared.Command;

namespace UdpLink.Client.Command
{
    public class RebootCmdBuilder : ICmdBuilder
    {
        public string CommandKey => "reboot";

        public CommandBase BuildCommand(string cmd)
        {
            return new RebootCommand();
        }
    }
}
