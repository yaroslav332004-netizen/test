using TrainingDiary.Exercises;
using TrainingDiary.Ordering;
using TrainingDiary.Storage;

namespace TrainingDiary.Workout;

/// <summary>
/// Ядро щоденника. Не знає ані про консоль, ані про файли:
/// звідки брати вправи й у якому порядку — вирішують ті, кого йому передали.
/// </summary>
public class WorkoutSession
{
    private readonly IExerciseStorage storage;
    private readonly IExerciseOrder order;
    private readonly List<Exercise> skipped = new();

    private WorkoutSummary summary = new(0, 0, 0);

    public WorkoutSession(IExerciseStorage storage, IExerciseOrder order)
    {
        this.storage = storage;
        this.order = order;
    }

    public string OrderName => order.Name;

    /// <summary>Вправи в порядку, який задав порядок перебору.</summary>
    public async Task<List<Exercise>> ExercisesAsync() =>
        order.Order(await storage.LoadAsync());

    /// <summary>
    /// Зараховує результат вправи й повертає навантаження.
    /// null — введення не вдалося розпізнати, вправу пропущено.
    /// </summary>
    public double? Record(Exercise exercise, string given)
    {
        var load = exercise.CalculateLoad(given);

        summary += load;

        if (load is null)
            skipped.Add(exercise);

        return load;
    }

    /// <summary>Вправи, які не вдалося зарахувати: їх варто повторити.</summary>
    public IReadOnlyList<Exercise> Skipped => skipped;

    public WorkoutSummary Summary => summary;
}
