using System.Runtime.InteropServices;
using System.Text;
using TrainingDiary.Exercises;

Console.OutputEncoding = Encoding.UTF8;
Console.WriteLine("Щоденник тренувань");
Console.WriteLine($"Платформа: {RuntimeInformation.OSDescription}");

var exercises = new List<Exercise>
{
    new Exercise(1, "Присідання", "повторень", 0.5) { Category = "Сила" },
    new Exercise(2, "Планка", "хв", 3) { Category = "Сила" },
    new Exercise(3, "Біг", "км", 10) { Category = "Кардіо" },
    new Exercise(4, "Скакалка", "хв", 8) { Category = "Кардіо" }
};

Console.WriteLine($"Вправ у щоденнику: {exercises.Count}.");

// Перелік збирають у буфер і друкують одним викликом.
var list = new StringBuilder();

foreach (var exercise in exercises)
    list.AppendLine($" {exercise}");

Console.Write(list.ToString());

// Для кожної вправи питаємо кількість і обчислюємо навантаження.
foreach (var exercise in exercises)
{
    Console.WriteLine();
    Console.WriteLine($"{exercise.Name}");
    Console.Write($"Скільки ({exercise.Unit}): ");
    var given = Console.ReadLine() ?? "";

    var load = exercise.CalculateLoad(given);

    Console.WriteLine(load is null
        ? "Не вдалося розпізнати кількість: потрібне невід’ємне число."
        : $"Навантаження: {load:0.##} балів.");
}

// Пошук вправи за назвою: тут видно різницю між способами порівняння рядків.
Console.WriteLine();
Console.Write("Назва вправи для пошуку (Enter — пропустити): ");
var query = Console.ReadLine() ?? "";

if (query.Trim().Length > 0)
{
    var found = exercises.Find(e => e.HasName(query));
    Console.WriteLine(found is null ? "Такої вправи немає." : $"Знайдено: {found}");
}
