using AQ.Domain;

namespace AQ.Tests;

public class DefaultCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Register<AchievementClass>(() => new()
        {
            Name = fixture.Create<string>(),
            Unit = fixture.Create<string>(),
        });
        fixture.Customize<DateOnly>(o => o.FromFactory((DateTime dt) => DateOnly.FromDateTime(dt)));
        fixture.Register<Achievement>(() => new()
        {
            CompletedDate = fixture.Create<DateOnly>(),
            Quantity = fixture.Create<int>(),
            Notes = fixture.Create<string>(),
        });
    }
}

public class DefaultAutoDataAttribute() : AutoDataAttribute(() => new Fixture()
    .Customize(new DefaultCustomization()));