namespace UdpLink.Server.Command
{
    using System.Diagnostics;

    using UdpLink.Shared.Command;

    public class RebootCommandHandler : CommandHandlerBase<RebootCommand>
    {
        public override string Handle(RebootCommand payload)
        {
            var psi = new ProcessStartInfo("shutdown", "/r /f");
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            Process.Start(psi);
            return $"Reboot launched as '{ psi.FileName} { psi.Arguments}'";
        }
    }
}
