using TrainingDiary.Exercises;

namespace TrainingDiary.Storage;

/// <summary>Тимчасове сховище: вправи задано просто в коді.</summary>
public class SampleExerciseStorage : IExerciseStorage
{
    public List<Exercise> Load() => Exercises();

    /// <summary>Зразкові вправи. Статичний метод: вони знадобляться й тому, хто до сховища не звертається.</summary>
    public static List<Exercise> Exercises() => new()
    {
        new RepsExercise(1, "Присідання", 0.5, 2, "Звичайні", "Зі стрибком", "З обтяженням")
            { Category = "Сила" },
        new TimedExercise(2, "Планка", 3) { Category = "Сила" },
        new DistanceExercise(3, "Біг", 10, 5) { Category = "Кардіо" },
        new TimedExercise(4, "Скакалка", 8, 60) { Category = "Кардіо" },
        new DistanceExercise(5, "Велосипед", 4) { Category = "Кардіо" }
    };
}
