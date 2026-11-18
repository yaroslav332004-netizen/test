using TrainingDiary.Exercises;

namespace TrainingDiary.Storage;

/// <summary>Звідки беруться вправи: файл чи пам’ять — ядро не знає.</summary>
public interface IExerciseStorage
{
    Task<List<Exercise>> LoadAsync();
    Task SaveAsync(List<Exercise> exercises);
}
