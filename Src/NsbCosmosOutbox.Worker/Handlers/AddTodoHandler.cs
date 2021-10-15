using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using NsbCosmosOutbox.Shared.Commands;
using NsbCosmosOutbox.Worker.Events;
using NsbCosmosOutbox.Worker.Models;
using NServiceBus;

namespace NsbCosmosOutbox.Worker.Handlers
{
    public class AddTodoHandler : IHandleMessages<AddTodo>
    {
        private readonly ILogger<AddTodoHandler> _logger;

        public AddTodoHandler(ILogger<AddTodoHandler> logger)
        {
            _logger = logger;
        }

        public async Task Handle(AddTodo message, IMessageHandlerContext context)
        {
            _logger.LogInformation("Received AddTodo message {@Message} - {MessageId}", message, context.MessageId);
            
            var session = context.SynchronizedStorageSession.CosmosPersistenceSession();
            var requestOptions = new TransactionalBatchItemRequestOptions
            {
                EnableContentResponseOnWrite = false
            };

            session.Batch.CreateItem(new Todo
            {
                Id = Guid.NewGuid(),
                TodoId = message.TodoId,
                Task = message.Task
            }, requestOptions);
            
            await context.Publish(new TodoAdded(message.TodoId));
        }
    }
}