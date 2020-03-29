namespace UdpLink.Shared.Command
{
    public class EchoCommand : CommandBase
    {
        public override CommandType CommandType => CommandType.Echo;

        public string Text { get; set; }
    }
}
