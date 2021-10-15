using System;
using NsbCosmosOutbox.Shared;
using NServiceBus;

namespace NsbCosmosOutbox.Worker.Events
{
    public class TodoAdded : IEvent, IProvideTodoId
    {
        public TodoAdded(Guid todoId)
        {
            TodoId = todoId;
        }

        public Guid TodoId { get; }
    }
}