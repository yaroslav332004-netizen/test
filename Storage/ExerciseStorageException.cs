namespace TrainingDiary.Storage;

/// <summary>Помилка читання чи запису щоденника, зрозуміла людині.</summary>
public class ExerciseStorageException : Exception
{
    public ExerciseStorageException(string message, Exception inner)
        : base(message, inner) { }
}
