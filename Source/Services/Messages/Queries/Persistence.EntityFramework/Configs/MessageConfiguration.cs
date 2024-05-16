using Messages.Queries.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Messages.Queries.Persistence.EntityFramework.Configs;

public sealed record MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder) => 
        builder.Property(x => x.Id).ValueGeneratedNever();
}