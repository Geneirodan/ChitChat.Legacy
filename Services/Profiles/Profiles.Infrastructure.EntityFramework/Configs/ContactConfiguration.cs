using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profiles.Domain.Aggregates;

namespace Profiles.Infrastructure.EntityFramework.Configs;

internal sealed class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.FirstName).HasMaxLength(64);
        builder.Property(x => x.LastName).HasMaxLength(64);
        builder.HasQueryFilter(x => x.IsDeleted == false);
    }
}