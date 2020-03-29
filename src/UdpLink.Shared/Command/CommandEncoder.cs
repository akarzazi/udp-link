namespace UdpLink.Shared.Command
{
    using System.Text;

    using Newtonsoft.Json;

    public static class CommandEncoder
    {
        private static JsonSerializerSettings _settings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All
        };

        public static byte[] Encode(CommandBase payload)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payload, _settings));
        }

        public static CommandBase Decode(byte[] payloadBytes)
        {
            var str = Encoding.UTF8.GetString(payloadBytes);
            return JsonConvert.DeserializeObject<CommandBase>(str, _settings);
        }
    }
}
