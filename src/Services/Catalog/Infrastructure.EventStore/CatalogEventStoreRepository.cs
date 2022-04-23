using Application.EventStore;
using Application.EventStore.Events;
using Domain.Aggregates;
using Infrastructure.EventStore.Abstractions;
using Infrastructure.EventStore.Contexts;

namespace Infrastructure.EventStore;

public class CatalogEventStoreRepository : EventStoreRepository<Catalog, CatalogStoreEvent, CatalogSnapshot, Guid>, ICatalogEventStoreRepository
{
    public CatalogEventStoreRepository(EventStoreDbContext dbContext)
        : base(dbContext) { }
}