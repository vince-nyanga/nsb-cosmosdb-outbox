using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NsbCosmosOutbox.Worker.Events;
using NServiceBus;

namespace NsbCosmosOutbox.Worker.Handlers
{
    public class TodoAddedHandler : IHandleMessages<TodoAdded>
    {
        private readonly ILogger<TodoAddedHandler> _logger;

        public TodoAddedHandler(ILogger<TodoAddedHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(TodoAdded message, IMessageHandlerContext context)
        {
            // This handler may dispatch other commands e.g. sending notification to the user
            _logger.LogInformation("Received TodoAdded message: {@Message} - {MessageId}", message, context.MessageId);
           return Task.CompletedTask;
        }
    }
}