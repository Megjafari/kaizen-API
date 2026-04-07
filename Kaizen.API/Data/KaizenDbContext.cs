using Microsoft.EntityFrameworkCore;
using Kaizen.API.Models;

namespace Kaizen.API.Data;

public class KaizenDbContext : DbContext
{
    public KaizenDbContext(DbContextOptions<KaizenDbContext> options) : base(options) { }

    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
    public DbSet<WorkoutTemplate> WorkoutTemplates => Set<WorkoutTemplate>();
    public DbSet<TemplateExercise> TemplateExercises => Set<TemplateExercise>();
    public DbSet<WorkoutLog> WorkoutLogs => Set<WorkoutLog>();
    public DbSet<ExerciseLog> ExerciseLogs => Set<ExerciseLog>();
    public DbSet<Ingredient> Ingredients => Set<Ingredient>();
    public DbSet<FoodLog> FoodLogs => Set<FoodLog>();
    public DbSet<WeightLog> WeightLogs => Set<WeightLog>();
    public DbSet<ProgressPhoto> ProgressPhotos => Set<ProgressPhoto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserProfile>()
            .HasIndex(u => u.UserId)
            .IsUnique();

        modelBuilder.Entity<WorkoutLog>()
            .HasIndex(w => new { w.UserId, w.Date });

        modelBuilder.Entity<FoodLog>()
            .HasIndex(f => new { f.UserId, f.Date });

        modelBuilder.Entity<WeightLog>()
            .HasIndex(w => new { w.UserId, w.Date });
    }
}