﻿using Application.Abstractions;
using Contracts.Services.Catalog;
using MassTransit;

namespace Application.UseCases.Events;

public interface IProjectCatalogItemCardWhenCatalogItemAddedInteractor : IInteractor<DomainEvent.CatalogItemAdded> { }

public class ProjectCatalogItemCardWhenCatalogItemAddedInteractor : IProjectCatalogItemCardWhenCatalogItemAddedInteractor
{
    private readonly IProjectionGateway<Projection.CatalogItemCard> _projectionGateway;

    public ProjectCatalogItemCardWhenCatalogItemAddedInteractor(IProjectionGateway<Projection.CatalogItemCard> projectionGateway)
    {
        _projectionGateway = projectionGateway;
    }

    public async Task InteractAsync(DomainEvent.CatalogItemAdded @event, CancellationToken cancellationToken)
    {
        Projection.CatalogItemCard card = new(
            @event.ItemId,
            @event.CatalogId,
            @event.Product,
            @event.UnitPrice,
            "image url",
            false);

        await _projectionGateway.InsertAsync(card, cancellationToken);
    }
}