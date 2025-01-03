This is Achievement Quantifier - a .NET CLI tool for tracking and managing achievements.

Define achievement classes:

```shell
> aq class add -n "Do programming" -u "Git commits"
info: AQ.Console.Commands.AddAchievementClass[0]
      Added 'AchievementClass { Id: 1, Name: Do programming, Unit: Git commits }'.
```

Log achievements with custom quantities and arbitrary dates:

```shell
> aq achievement add -n "Do programming" -q 4 -d "31/12/2024"
info: AQ.Console.Commands.AddAchievement[0]
      Added achievement: Achievement { Id: 1, Class: Do programming, Quantity: 4 Unit: Git commits, CompletedDate: 31/12/2024 }.
> aq achievement add -n "Do programming" -d "01/01/2025" 
info: AQ.Console.Commands.AddAchievement[0]
      Added achievement: Achievement { Id: 2, Class: Do programming, Quantity: 1 Unit: Git commits, CompletedDate: 1/1/2025 }.
> aq achievement add -n "Do programming" -q 2 -d "02/01/2025"
info: AQ.Console.Commands.AddAchievement[0]
      Added achievement: Achievement { Id: 3, Class: Do programming, Quantity: 2 Unit: Git commits, CompletedDate: 2/1/2025 }.
```

View the progress:

```shell
> aq achievement list
info: AQ.Console.Commands.ListAchievements[0]
      Found 3 achievements.
Achievement { Id: 1, Class: Do programming, Quantity: 4 Unit: Git commits, CompletedDate: 31/12/2024 }
Achievement { Id: 2, Class: Do programming, Quantity: 1 Unit: Git commits, CompletedDate: 1/1/2025 }
Achievement { Id: 3, Class: Do programming, Quantity: 2 Unit: Git commits, CompletedDate: 2/1/2025 }
```
