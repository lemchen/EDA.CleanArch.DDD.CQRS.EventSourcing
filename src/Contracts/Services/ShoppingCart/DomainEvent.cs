﻿using Contracts.Abstractions.Messages;
using Contracts.DataTransferObjects;

namespace Contracts.Services.ShoppingCart;

public static class DomainEvent
{
    public record CartCreated(Guid CartId, Guid CustomerId, string Status) : Message, IEvent;

    public record CartItemAdded(Guid CartId, Guid ItemId, Guid InventoryId, Dto.Product Product, ushort Quantity, decimal UnitPrice, decimal NewCartTotal) : Message, IEvent;

    public record CartItemIncreased(Guid CartId, Guid ItemId, ushort NewQuantity, decimal UnitPrice, decimal NewCartTotal) : Message, IEvent;

    public record CartItemDecreased(Guid CartId, Guid ItemId, ushort NewQuantity, decimal UnitPrice, decimal NewCartTotal) : Message, IEvent;

    public record CartItemRemoved(Guid CartId, Guid ItemId, decimal UnitPrice, int Quantity, decimal NewCartTotal) : Message, IEvent;

    public record CartCheckedOut(Guid CartId, string Status) : Message, IEvent;

    public record ShippingAddressAdded(Guid CartId, Dto.Address Address) : Message, IEvent;

    public record BillingAddressAdded(Guid CartId, Dto.Address Address) : Message, IEvent;

    public record CartDiscarded(Guid CartId, string Status) : Message, IEvent;

    public record CreditCardAdded(Guid CartId, Guid MethodId, decimal Amount, Dto.CreditCard CreditCard) : Message, IEvent;

    public record DebitCardAdded(Guid CartId, Guid MethodId, decimal Amount, Dto.DebitCard DebitCard) : Message, IEvent;

    public record PayPalAdded(Guid CartId, Guid MethodId, decimal Amount, Dto.PayPal PayPal) : Message, IEvent;
}