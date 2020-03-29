using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using UdpLink.Shared.Command;

namespace UdpLink.Server.Command
{
    public class CommandExecutorService
    {
        private readonly ILogger<CommandExecutorService> _logger;
        private readonly IDictionary<Type, ICommandHandler> _payloadHandlers;

        public CommandExecutorService(ILogger<CommandExecutorService> logger, IEnumerable<ICommandHandler> commandHandlers)
        {
            _logger = logger;
            _payloadHandlers = commandHandlers.ToDictionary(p => p.HandledType);
        }

        //public static readonly IDictionary<Type, ICommandHandler> PayloadHandlers;

        //static CommandExecutor()
        //{
        //    PayloadHandlers = typeof(CommandExecutor).Assembly.GetTypes()
        //           .Where(t => typeof(ICommandHandler).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
        //           .Select(t => (ICommandHandler)Activator.CreateInstance(t))
        //           .ToDictionary(p => p.HandledType);
        //}

        public string ExecuteCommand(CommandBase payload)
        {
            _logger.LogInformation($"Attempted payload {JsonConvert.SerializeObject(payload)}");

            var payloadType = payload.GetType();
            if (_payloadHandlers.TryGetValue(payloadType, out ICommandHandler payloadHandler))
            {
                return payloadHandler.Handle(payload);
            }

            var errmsg = $"Could not find handler for payloadType {payloadType}";
            _logger.LogError(errmsg);
            return errmsg;
        }
    }
}
