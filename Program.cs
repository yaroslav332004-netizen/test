using System.Runtime.InteropServices;
using TrainingDiary.Exercises;
using TrainingDiary.Ordering;
using TrainingDiary.Storage;
using TrainingDiary.Workout;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("Щоденник тренувань");
Console.WriteLine($"Платформа: {RuntimeInformation.OSDescription}");

// Path.Combine складає шлях із правильним роздільником:
// \ у Windows, / у Linux і macOS.
var file = Path.Combine(AppContext.BaseDirectory, "data", "diary.json");
var storage = new JsonExerciseStorage(file);

try
{
    // Ознака першого запуску — відсутній файл, а не порожній перелік:
    // порожнім він буває й тоді, коли у файлі є невідомі різновиди.
    if (!File.Exists(storage.Path))
    {
        await storage.SaveAsync(SampleExerciseStorage.Exercises());
        Console.WriteLine($"Створено зразковий щоденник: {storage.Path}");
    }

    // Тут і тільки тут вирішують, з чого складається щоденник.
    var session = new WorkoutSession(storage, new ByCategoryOrder());
    var exercises = await session.ExercisesAsync();

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
}
catch (ExerciseStorageException ex)
{
    Console.WriteLine(ex.Message);
    Console.WriteLine("Виправте або видаліть файл щоденника й запустіть ще раз.");
}
