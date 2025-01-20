namespace AQ.Domain;

public class AchievementClass
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string Unit { get; set; }
    public virtual ICollection<Achievement> Achievements { get; set; } = new List<Achievement>();
    public override string ToString() => $"{nameof(AchievementClass)} {{ Id: {Id}, Name: {Name}, Unit: {Unit} }}";
}
