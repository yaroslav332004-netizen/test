using System.Text;
using Spectre.Console;
using TrainingDiary.Exercises;
using TrainingDiary.Ordering;
using TrainingDiary.Storage;
using TrainingDiary.Workout;

Console.OutputEncoding = Encoding.UTF8;

AnsiConsole.Write(new Rule("[bold]Щоденник тренувань[/]")
{
    Justification = Justify.Left
});

var file = Path.Combine(AppContext.BaseDirectory, "data", "diary.json");
var storage = new JsonExerciseStorage(file);

try
{
    // Ознака першого запуску — відсутній файл, а не порожній перелік (ЛР 5).
    if (!File.Exists(storage.Path))
    {
        await storage.SaveAsync(SampleExerciseStorage.Exercises());
        AnsiConsole.MarkupLineInterpolated(
            $"[grey]Створено зразковий щоденник: {storage.Path}[/]");
    }

    var session = new WorkoutSession(storage, new ByCategoryOrder());
    var ordered = await session.ExercisesAsync();

    AnsiConsole.MarkupLineInterpolated(
        $"Вправ: [bold]{ordered.Count}[/], порядок: {session.OrderName}.");

    for (var i = 0; i < ordered.Count; i++)
    {
        var exercise = ordered[i];

        AnsiConsole.WriteLine();
        AnsiConsole.Write(new Panel(new Text(exercise.Name))
        {
            Header = new PanelHeader(
                $"{i + 1} з {ordered.Count} · {Markup.Escape(exercise.Category)}"),
            Border = BoxBorder.Rounded,
            Expand = true
        });

        AnsiConsole.MarkupLineInterpolated($"[grey]Навантаження: {exercise.LoadRule}[/]");

        var given = Ask(exercise);

        // Рахує ядро. Оболонка лише обирає колір.
        var load = session.Record(exercise, given);

        if (load is null)
            AnsiConsole.MarkupLine("[red]✗ Не вдалося розпізнати кількість — вправу пропущено.[/]");
        else
            AnsiConsole.MarkupLineInterpolated($"[green]✓ Зараховано:[/] [bold]{load:0.##}[/] балів");
    }

    ShowSummary(session);
    return 0;
}
catch (ExerciseStorageException ex)
{
    AnsiConsole.MarkupLineInterpolated($"[red]{ex.Message}[/]");
    AnsiConsole.MarkupLine("Виправте або видаліть файл щоденника.");
    return 1;
}

// Вибір варіанта стрілками — лише для вправ із варіантами й лише
// в інтерактивному терміналі з ANSI. Інакше варіанти друкуємо списком
// і лишаємо той, що записано у вправі; кількість читаємо рядком.
string Ask(Exercise exercise)
{
    var capabilities = AnsiConsole.Profile.Capabilities;
    // Введення з файлу чи каналу: натискань клавіш не буде.
    var interactive = capabilities.Interactive && !Console.IsInputRedirected;
    var canSelect = interactive && capabilities.Ansi;

    if (exercise is RepsExercise reps && reps.Variants.Count > 1)
    {
        if (canSelect)
        {
            var number = AnsiConsole.Prompt(
                new SelectionPrompt<int>()
                    .Title("Варіант виконання:")
                    .AddChoices(Enumerable.Range(1, reps.Variants.Count))
                    .UseConverter(n => Markup.Escape($"{n}. {reps[n]}")));

            reps.Variant = number;

            // Меню після вибору зникає — лишаємо слід, що саме обрано.
            AnsiConsole.MarkupLineInterpolated(
                $"Варіант виконання: [blue]{number}. {reps[number]}[/]");
        }
        else
        {
            for (var n = 1; n <= reps.Variants.Count; n++)
                AnsiConsole.MarkupLineInterpolated(
                    $" {n}. {reps[n]}{(n == reps.Variant ? "  ← обрано" : "")}");
        }
    }

    if (!interactive)
    {
        AnsiConsole.Markup($"{Markup.Escape(exercise.Prompt)}: ");
        return Console.ReadLine() ?? "";
    }

    var prompt = new TextPrompt<string>($"{Markup.Escape(exercise.Prompt)}:");
    return AnsiConsole.Prompt(prompt.AllowEmpty());
}

void ShowSummary(WorkoutSession session)
{
    var result = session.Summary;

    AnsiConsole.WriteLine();
    AnsiConsole.Write(new Rule("[bold]Підсумок[/]")
    {
        Justification = Justify.Left
    });

    AnsiConsole.MarkupLineInterpolated(
        $"[green]{result.Done}[/] виконано, {result.Skipped} пропущено · навантаження [bold]{result.TotalLoad:0.##}[/] б., у середньому {result.AverageLoad:0.##}");

    if (result.Skipped == 0)
        return;

    var skipped = new Table { Border = TableBorder.Rounded };
    skipped.AddColumn("Повторити варто");
    skipped.AddColumn("Як рахується навантаження");

    foreach (var exercise in session.Skipped)
        skipped.AddRow(
            new Text(exercise.Name),
            new Text(exercise.LoadRule));

    AnsiConsole.WriteLine();
    AnsiConsole.Write(skipped);
}
