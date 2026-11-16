using System.Runtime.InteropServices;
using TrainingDiary.Exercises;
using TrainingDiary.Ordering;
using TrainingDiary.Storage;
using TrainingDiary.Workout;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("Щоденник тренувань");
Console.WriteLine($"Платформа: {RuntimeInformation.OSDescription}");

// Тут і тільки тут вирішують, з чого складається щоденник.
IExerciseStorage storage = new SampleExerciseStorage();
IExerciseOrder order = new ByCategoryOrder();

var session = new WorkoutSession(storage, order);
var exercises = session.Exercises();

Console.WriteLine($"Вправ: {exercises.Count}, порядок: {session.OrderName}.");

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
    var load = session.Record(exercise, given);

    Console.WriteLine(load is null
        ? "Не вдалося розпізнати кількість — вправу пропущено."
        : $"Зараховано: {load:0.##} балів.");
}

var summary = session.Summary;

Console.WriteLine();
Console.WriteLine(
    $"Підсумок: виконано {summary.Done}, пропущено {summary.Skipped}, " +
    $"загальне навантаження {summary.TotalLoad:0.##} балів, " +
    $"у середньому {summary.AverageLoad:0.##}.");

if (session.Skipped.Count > 0)
{
    Console.WriteLine("Повторити варто:");

    foreach (var exercise in session.Skipped)
        Console.WriteLine($" {exercise.Name}");
}
