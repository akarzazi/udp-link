using System;
using System.Collections.Generic;
using System.Text;

namespace UdpLink.Shared.Command
{
    public class PowershellCommand : CommandBase
    {
        public override CommandType CommandType => CommandType.Powershell;

        public string CommandText { get; set; }
    }
}
