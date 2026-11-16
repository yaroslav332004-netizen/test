using System.Runtime.InteropServices;
using TrainingDiary.Exercises;
using TrainingDiary.Workout;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("Щоденник тренувань");
Console.WriteLine($"Платформа: {RuntimeInformation.OSDescription}");

var exercises = new List<Exercise>
{
    new RepsExercise(1, "Присідання", 0.5, 2, "Звичайні", "Зі стрибком", "З обтяженням")
        { Category = "Сила" },
    new TimedExercise(2, "Планка", 3) { Category = "Сила" },
    new DistanceExercise(3, "Біг", 10, 5) { Category = "Кардіо" },
    new TimedExercise(4, "Скакалка", 8, 60) { Category = "Кардіо" },
    new DistanceExercise(5, "Велосипед", 4) { Category = "Кардіо" }
};

Console.WriteLine($"Вправ у щоденнику: {exercises.Count}.");

var summary = new WorkoutSummary(0, 0, 0);

foreach (var exercise in exercises)
{
    Console.WriteLine();
    Console.WriteLine(exercise);
    Console.WriteLine($"Навантаження: {exercise.LoadRule}");

    if (exercise is RepsExercise reps)
        for (var n = 1; n <= reps.Variants.Count; n++)
            Console.WriteLine($" {n}. {reps[n]}{(n == reps.Variant ? "  ← обрано" : "")}");

    Console.Write($"{exercise.Prompt}: ");
    var given = Console.ReadLine() ?? "";
    var load = exercise.CalculateLoad(given);

    summary += load;

    Console.WriteLine(load is null
        ? "Не вдалося розпізнати кількість — вправу пропущено."
        : $"Зараховано: {load:0.##} балів.");
}

Console.WriteLine();
Console.WriteLine(
    $"Підсумок: виконано {summary.Done}, пропущено {summary.Skipped}, " +
    $"загальне навантаження {summary.TotalLoad:0.##} балів, " +
    $"у середньому {summary.AverageLoad:0.##}.");
