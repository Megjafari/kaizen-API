namespace Kaizen.API.DTOs;

public class CreateWorkoutLogDto
{
    public DateTime Date { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public List<CreateExerciseLogDto> Exercises { get; set; } = [];
}

public class CreateExerciseLogDto
{
    public string ExerciseName { get; set; } = string.Empty;
    public int Sets { get; set; }
    public int Reps { get; set; }
    public decimal Weight { get; set; }
}