using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AQ.Domain;

public class Achievement
{
    public long Id { get; set; }
    public required DateOnly CompletedDate { get; set; }
    public required int Quantity { get; set; }
    public required string Notes { get; set; }
    public long AchievementClassId { get; set; }
    public AchievementClass AchievementClass { get; set; } = null!;
    public override string ToString()
    {
        return new StringBuilder()
            .Append(nameof(Achievement))
            .Append(" { Id: ")
            .Append(Id)
            .Append(", Class: ")
            .Append(AchievementClass.Name)
            .Append(", Quantity: ")
            .Append(Quantity)
            .Append(", Unit: ")
            .Append(AchievementClass.Unit)
            .Append(", CompletedDate: ")
            .Append(CompletedDate)
            .Append(", Notes: ")
            .Append(Notes)
            .Append(" }")
            .ToString();
    }
}

public class AchievementConfiguration : IEntityTypeConfiguration<Achievement>
{
    public void Configure(EntityTypeBuilder<Achievement> builder)
    {
        builder.ToTable("Achievement");
        builder.HasOne(x => x.AchievementClass).WithMany(x => x.Achievements)
            .HasForeignKey(x => x.AchievementClassId)
            .OnDelete(DeleteBehavior.ClientSetNull);
        builder.Property(x => x.Notes).HasMaxLength(1000);
    }
}