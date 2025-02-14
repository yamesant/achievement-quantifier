using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AQ.Domain;

public class AchievementClass
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string Unit { get; set; }
    public ICollection<Achievement> Achievements { get; set; } = null!;
    public override string ToString() => $"{nameof(AchievementClass)} {{ Id: {Id}, Name: {Name}, Unit: {Unit} }}";
}

public class AchievementClassConfiguration : IEntityTypeConfiguration<AchievementClass>
{
    public void Configure(EntityTypeBuilder<AchievementClass> builder)
    {
        builder.ToTable("AchievementClass");
        builder.HasIndex(x => x.Name, "IX_AchievementClass_Name").IsUnique();
        builder.Property(x => x.Name).HasMaxLength(200);
        builder.Property(x => x.Unit).HasMaxLength(200);
    }
}
