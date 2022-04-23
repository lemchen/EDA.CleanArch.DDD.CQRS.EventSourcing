using Application.EventStore.Events;
using Infrastructure.EventStore.Contexts.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EventStore.Contexts.Configurations;

public class UserStoreEventConfiguration : IEntityTypeConfiguration<UserStoreEvent>
{
    public void Configure(EntityTypeBuilder<UserStoreEvent> builder)
    {
        builder.HasKey(storeEvent => storeEvent.Version);

        builder
            .Property(storeEvent => storeEvent.AggregateId)
            .IsRequired();

        builder
            .Property(storeEvent => storeEvent.AggregateName)
            .HasMaxLength(30)
            .IsRequired();

        builder
            .Property(storeEvent => storeEvent.EventName)
            .HasMaxLength(50)
            .IsRequired();

        builder
            .Property(storeEvent => storeEvent.Event)
            .HasConversion<EventConverter>()
            .IsRequired();
    }
}