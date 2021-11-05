﻿using System;
using Application.EventSourcing.EventStore;
using Application.EventSourcing.Projections;
using FluentValidation;
using Infrastructure.Abstractions.EventSourcing.Projections.Contexts;
using Infrastructure.Abstractions.EventSourcing.Projections.Contexts.BsonSerializers;
using Infrastructure.DependencyInjection.Filters;
using Infrastructure.DependencyInjection.Observers;
using Infrastructure.DependencyInjection.Options;
using Infrastructure.EventSourcing.EventStore;
using Infrastructure.EventSourcing.EventStore.Contexts;
using Infrastructure.EventSourcing.Projections;
using Infrastructure.EventSourcing.Projections.Contexts;
using MassTransit;
using Messages.Abstractions;
using Messages.JsonConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Newtonsoft.Json;

namespace Infrastructure.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    private static readonly RabbitMqOptions Options = new();

    public static IServiceCollection AddMassTransitWithRabbitMq(this IServiceCollection services, Action<RabbitMqOptions> optionsAction)
        => services.AddMassTransit(cfg =>
            {
                optionsAction(Options);

                cfg.SetKebabCaseEndpointNameFormatter();

                cfg.AddCommandConsumers();
                cfg.AddEventConsumers();
                cfg.AddQueryConsumers();

                cfg.UsingRabbitMq((context, bus) =>
                {
                    bus.Host(
                        host: Options.Host,
                        port: Options.Port,
                        virtualHost: Options.VirtualHost,
                        host =>
                        {
                            host.Username(Options.Username);
                            host.Password(Options.Password);
                        });

                    bus.MessageTopology.SetEntityNameFormatter(new KebabCaseEntityNameFormatter());
                    bus.UseConsumeFilter(typeof(MessageValidatorFilter<>), context);
                    bus.ConnectConsumeObserver(new LoggingConsumeObserver());
                    bus.ConnectPublishObserver(new LoggingPublishObserver());
                    bus.ConnectSendObserver(new LoggingSendObserver());
                    bus.ConfigureEventReceiveEndpoints(context);
                    bus.ConfigureEndpoints(context);
                    
                    
                    bus.ConfigureJsonSerializer(settings =>  new JsonSerializerSettings());
                    
                    bus.ConfigureJsonSerializer(settings =>
                    {
                        settings.Converters.Add(new DateOnlyJsonConverter()); 
                        settings.Converters.Add(new ExpirationDateOnlyJsonConverter()); 
                        return settings;
                    });
                    
                    bus.ConfigureJsonDeserializer(settings =>
                    {
                        settings.Converters.Add(new DateOnlyJsonConverter()); 
                        settings.Converters.Add(new ExpirationDateOnlyJsonConverter()); 
                        return settings;
                    });
                });
            })
            .AddMassTransitHostedService();

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        => services
            .AddScoped<IShoppingCartEventStoreService, ShoppingCartEventStoreService>()
            .AddScoped<IShoppingCartProjectionsService, ShoppingCartProjectionsService>();

    public static IServiceCollection AddEventStoreDbContext(this IServiceCollection services)
        => services
            .AddScoped<DbContext, EventStoreDbContext>()
            .AddDbContext<EventStoreDbContext>();

    public static IServiceCollection AddProjectionsDbContext(this IServiceCollection services)
    {
        BsonSerializer.RegisterSerializer(new DateOnlyBsonSerializer());
        //BsonSerializer.RegisterSerializer(new ExpirationDateOnlyBsonSerializer());
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.CSharpLegacy));
        
        return services.AddScoped<IMongoDbContext, ProjectionsDbContext>();
    }

    public static IServiceCollection AddEventStoreRepositories(this IServiceCollection services)
        => services.AddScoped<IShoppingCartEventStoreRepository, ShoppingCartEventStoreRepository>();

    public static IServiceCollection AddProjectionsRepositories(this IServiceCollection services)
        => services.AddScoped<IShoppingCartProjectionsRepository, ShoppingCartProjectionsRepository>();

    public static IServiceCollection AddMessageFluentValidation(this IServiceCollection services)
        => services.AddValidatorsFromAssemblyContaining(typeof(IMessage));

    public static OptionsBuilder<SqlServerRetryingOptions> ConfigureSqlServerRetryingOptions(this IServiceCollection services, IConfigurationSection section)
        => services
            .AddOptions<SqlServerRetryingOptions>()
            .Bind(section)
            .ValidateDataAnnotations()
            .ValidateOnStart();

    public static OptionsBuilder<EventStoreOptions> ConfigureEventStoreOptions(this IServiceCollection services, IConfigurationSection section)
        => services
            .AddOptions<EventStoreOptions>()
            .Bind(section)
            .ValidateDataAnnotations()
            .ValidateOnStart();
}