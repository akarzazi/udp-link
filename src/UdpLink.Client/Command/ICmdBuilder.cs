using UdpLink.Shared.Command;

namespace UdpLink.Client.Command
{
    public interface ICmdBuilder
    {
        string CommandKey { get; }

        CommandBase BuildCommand(string cmd);
    }
}
