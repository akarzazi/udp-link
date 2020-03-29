using System;
using System.Collections.Generic;
using System.Text;
using UdpLink.Shared.Command;

namespace UdpLink.Server.Command
{
    public class EchoCommandHandler : CommandHandlerBase<EchoCommand>
    {
        public override string Handle(EchoCommand payload)
        {
            return payload.Text;
        }
    }
}
