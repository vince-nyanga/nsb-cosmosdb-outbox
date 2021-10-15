using System;
using NServiceBus;

namespace NsbCosmosOutbox.Shared.Commands
{
    public class AddTodo : ICommand, IProvideTodoId
    {
        
        public Guid TodoId { get;  set; }
        public string Task { get; set; }
    }
}