using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Profiles.Domain.Aggregates;

namespace Profiles.Infrastructure.EntityFramework.Configs;

internal sealed class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.FirstName).HasMaxLength(64);
        builder.Property(x => x.LastName).HasMaxLength(64);
        builder.Property(x => x.Bio).HasMaxLength(2048);
        builder.Property(x => x.AvatarUrl).HasMaxLength(2048);
        builder.HasQueryFilter(x => x.IsDeleted == false);
    }
}