using System;
using System.Collections.Generic;

namespace AQ.Domain;

public partial class Achievement
{
    public int Id { get; set; }

    public int AchievementClassId { get; set; }

    public string CompletedDate { get; set; } = null!;

    public int Quantity { get; set; }

    public virtual AchievementClass AchievementClass { get; set; } = null!;
}
