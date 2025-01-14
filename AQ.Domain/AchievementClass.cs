namespace AQ.Domain;

public class AchievementClass
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string Unit { get; set; } = null!;
    public virtual ICollection<Achievement> Achievements { get; set; } = new List<Achievement>();
    public override string ToString() => $"{nameof(AchievementClass)} {{ Id: {Id}, Name: {Name}, Unit: {Unit} }}";
}
