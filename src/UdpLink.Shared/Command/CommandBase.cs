using System;

namespace UdpLink.Shared.Command
{
    public abstract class CommandBase
    {
        public abstract CommandType CommandType { get; }
    }
}
