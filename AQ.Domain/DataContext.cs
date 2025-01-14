using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AQ.Domain;

public partial class DataContext : DbContext
{
    public DataContext()
    {
    }

    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Achievement> Achievements { get; set; }

    public virtual DbSet<AchievementClass> AchievementClasses { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Achievement>(entity =>
        {
            entity.ToTable("Achievement");

            entity.HasOne(d => d.AchievementClass).WithMany(p => p.Achievements)
                .HasForeignKey(d => d.AchievementClassId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<AchievementClass>(entity =>
        {
            entity.ToTable("AchievementClass");

            entity.HasIndex(e => e.Name, "IX_AchievementClass_Name").IsUnique();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
