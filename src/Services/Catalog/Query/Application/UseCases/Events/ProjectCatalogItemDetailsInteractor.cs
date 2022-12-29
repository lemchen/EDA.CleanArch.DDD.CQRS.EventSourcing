﻿using Application.Abstractions;
using Contracts.Services.Catalog;

namespace Application.UseCases.Events;

public interface IProjectCatalogItemDetailsInteractor : IInteractor<DomainEvent.CatalogItemAdded> { }

public class ProjectCatalogItemDetailsInteractor : IProjectCatalogItemDetailsInteractor
{
    private readonly IProjectionGateway<Projection.CatalogItemDetails> _projectionGateway;

    public ProjectCatalogItemDetailsInteractor(IProjectionGateway<Projection.CatalogItemDetails> projectionGateway)
    {
        _projectionGateway = projectionGateway;
    }

    public async Task InteractAsync(DomainEvent.CatalogItemAdded @event, CancellationToken cancellationToken)
    {
        Projection.CatalogItemDetails card = new(
            @event.ItemId,
            @event.CatalogId,
            @event.Product,
            @event.UnitPrice,
            "image url",
            false);

        await _projectionGateway.InsertAsync(card, cancellationToken);
    }
}