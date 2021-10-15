using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using NsbCosmosOutbox.Shared;
using NServiceBus.Persistence.CosmosDB;
using NServiceBus.Pipeline;

namespace NsbCosmosOutbox.Worker.Behaviors
{
    public class TodoIdAsPartitionKeyBehavior : Behavior<IIncomingLogicalMessageContext>
    {
        public override Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
        {
            if (context.Message.Instance is IProvideTodoId provideTodoId)
            {
                var value = provideTodoId.TodoId.ToString();
                context.Extensions.Set(new PartitionKey(value));
            }

            return next();
        }
        
        public class Registration : RegisterStep
        {
            public Registration() :
                base(nameof(TodoIdAsPartitionKeyBehavior),
                    typeof(TodoIdAsPartitionKeyBehavior),
                    "Gets partition key from the logical message",
                    f => new TodoIdAsPartitionKeyBehavior())
            {
                InsertBefore(nameof(LogicalOutboxBehavior));
            }
        }
    }
}