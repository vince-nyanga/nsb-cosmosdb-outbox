using System;

namespace NsbCosmosOutbox.Shared
{
    public interface IProvideTodoId
    {
        Guid TodoId { get; }
    }
}