﻿using System;
using Application.UseCases.Commands;
using Application.UseCases.Events.Integrations;
using Application.UseCases.Events.Projections;
using Application.UseCases.Queries;
using MassTransit;
using Messages.Abstractions;
using Messages.Services.ShoppingCarts;

namespace Infrastructure.DependencyInjection.Extensions;

public static class RegistrationConfiguratorExtensions
{
    public static void AddCommandConsumers(this IRegistrationConfigurator cfg)
    {
        cfg.AddCommandConsumer<AddCartItemConsumer, Commands.AddCartItem>();
        cfg.AddCommandConsumer<CreateCartConsumer, Commands.CreateCart>();
        cfg.AddCommandConsumer<RemoveCartItemConsumer, Commands.RemoveCartItem>();
        cfg.AddCommandConsumer<ChangeBillingAddressConsumer, Commands.ChangeBillingAddress>();
        cfg.AddCommandConsumer<AddShippingAddressConsumer, Commands.AddShippingAddress>();
        cfg.AddCommandConsumer<AddCreditCardConsumer, Commands.AddCreditCard>();
        cfg.AddCommandConsumer<CheckOutCartConsumer, Commands.CheckOutCart>();
        cfg.AddCommandConsumer<UpdateCartItemQuantityConsumer, Commands.UpdateCartItemQuantity>();
    }

    public static void AddEventConsumers(this IRegistrationConfigurator cfg)
    {
        cfg.AddConsumer<ProjectCartDetailsWhenCartChangedConsumer>();
        cfg.AddConsumer<PublishCartSubmittedWhenCartCheckedOutConsumer>();
    }

    public static void AddQueryConsumers(this IRegistrationConfigurator cfg)
    {
        cfg.AddConsumer<GetShoppingCartConsumer>();
    }

    private static void AddCommandConsumer<TConsumer, TMessage>(this IRegistrationConfigurator configurator)
        where TConsumer : class, IConsumer
        where TMessage : class, IMessage
    {
        configurator
            .AddConsumer<TConsumer>()
            .Endpoint(endpoint => endpoint.ConfigureConsumeTopology = false);

        EndpointConvention.Map<TMessage>(new Uri($"exchange:{typeof(TMessage).ToKebabCaseString()}"));
    }
}