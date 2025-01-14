using System;
using System.Collections.Generic;

namespace AQ.Domain;

public partial class AchievementClass
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Unit { get; set; } = null!;

    public virtual ICollection<Achievement> Achievements { get; set; } = new List<Achievement>();
}
