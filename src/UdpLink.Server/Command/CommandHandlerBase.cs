using System;
using System.Collections.Generic;
using System.Text;
using UdpLink.Shared.Command;

namespace UdpLink.Server.Command
{
    public abstract class CommandHandlerBase<TCommand> : ICommandHandler<TCommand> where TCommand : CommandBase
    {
        public Type HandledType => typeof(TCommand);

        public string Handle(object payload)
        {
            return Handle((TCommand)payload);
        }

        public abstract string Handle(TCommand payload);
    }
}
