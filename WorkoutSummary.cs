namespace TrainingDiary.Workout;

/// <summary>Підсумок тренування: скільки вправ виконано, пропущено й яке загальне навантаження.</summary>
public record WorkoutSummary(int Done, int Skipped, double TotalLoad)
{
    public double AverageLoad => Done == 0 ? 0 : TotalLoad / Done;

    /// <summary>
    /// Додає до підсумку результат однієї вправи.
    /// null означає, що введення не вдалося розпізнати: вправу рахуємо пропущеною.
    /// </summary>
    public static WorkoutSummary operator +(WorkoutSummary s, double? load) =>
        load is null
            ? s with { Skipped = s.Skipped + 1 }
            : new(s.Done + 1, s.Skipped, s.TotalLoad + load.Value);
}
