using System.Text;

namespace AQ.Domain;

public class Achievement
{
    public long Id { get; set; }
    public long AchievementClassId { get; set; }
    public required DateOnly CompletedDate { get; set; }
    public required int Quantity { get; set; }
    public virtual AchievementClass AchievementClass { get; set; } = null!;
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
            .Append(" }")
            .ToString();
    }
}
