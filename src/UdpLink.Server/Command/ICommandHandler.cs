using System;

using UdpLink.Shared.Command;

namespace UdpLink.Server.Command
{
    public interface ICommandHandler<TCommand> : ICommandHandler where TCommand : CommandBase
    {
        string Handle(TCommand payload);
    }

    public interface ICommandHandler
    {
        Type HandledType { get; }

        string Handle(object payload);
    }
}
