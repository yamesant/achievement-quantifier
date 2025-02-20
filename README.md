# Description

This is Achievement Quantifier - a .NET CLI tool for tracking and managing achievements.

Define achievement classes:

```txt
> aq class add -n "Do programming" -u "Git commits"
Added 'AchievementClass { Id: 1, Name: Do programming, Unit: Git commits }'.
```

Log achievements with custom quantities and arbitrary dates:

```txt
> aq achievement add -n "Do programming" -q 4 -d "31/12/2024"
Added achievement: Achievement { Id: 1, Class: Do programming, Quantity: 4, Unit: Git commits, CompletedDate: 31/12/2024 }.
> aq achievement add -n "Do programming" -d "01/01/2025" 
Added achievement: Achievement { Id: 2, Class: Do programming, Quantity: 1, Unit: Git commits, CompletedDate: 1/1/2025 }.
> aq achievement add -n "Do programming" -q 2 -d "02/01/2025"
Added achievement: Achievement { Id: 3, Class: Do programming, Quantity: 2, Unit: Git commits, CompletedDate: 2/1/2025 }.
```

View the progress:

```txt
> aq stats
┌────────────────────────────────────┬───────┐
│ Statistic                          │ Value │
├────────────────────────────────────┼───────┤
│ Achievements Completed Today       │ 6     │
│ Achievements Completed Yesterday   │ 13    │
│ Achievements Completed Past 7 Days │ 42    │
└────────────────────────────────────┴───────┘
```

# Installation

1. Clone the repository
2. Set the environment: `export DOTNET_ENVIRONMENT=Production` (no environment set = production environment)
3. Update the database: `dotnet ef database update -s AQ.Console -p AQ.Domain`
4. Move to startup project folder: `cd AQ.Console`
5. Make a NuGet package: `dotnet pack`
6. Install as a global .NET CLI tool: `dotnet tool install AQ --global --add-source ./packages`
