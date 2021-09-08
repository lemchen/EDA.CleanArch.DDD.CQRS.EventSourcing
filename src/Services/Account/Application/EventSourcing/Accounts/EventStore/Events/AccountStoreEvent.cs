using System;
using Application.Abstractions.EventSourcing.EventStore.Events;
using Domain.Entities.Accounts;

namespace Application.EventSourcing.Accounts.EventStore.Events
{
    public record AccountStoreEvent : StoreEvent<Account, Guid>;
}