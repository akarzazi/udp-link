using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UdpLink.Shared.Command;

namespace UdpLink.Client.Command
{
    public class CmdParser
    {
        public static readonly IDictionary<string, ICmdBuilder> PayloadBuilders;

        static CmdParser()
        {
            PayloadBuilders = typeof(CmdParser).Assembly.GetTypes()
                   .Where(t => typeof(ICmdBuilder).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
                   .Select(t => (ICmdBuilder)Activator.CreateInstance(t))
                   .ToDictionary(p => p.CommandKey, StringComparer.OrdinalIgnoreCase);
        }

        public static CommandBase ParseCommandLine(string commandLineInput)
        {
            var splits = commandLineInput.Split(":", StringSplitOptions.RemoveEmptyEntries);
            return ParseCommand(splits[0], splits.ElementAtOrDefault(1));
        }

        public static CommandBase ParseCommand(string commandKey, string command)
        {
            if (PayloadBuilders.TryGetValue(commandKey, out var payloadCmdHandler))
            {
                return payloadCmdHandler.BuildCommand(command);
            }
            return null;
        }
    }
}
