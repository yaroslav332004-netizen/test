using System.Globalization;

namespace TrainingDiary.Exercises;

/// <summary>Одна вправа: назва, категорія, одиниця виміру й навантаження за одиницю.</summary>
public class Exercise
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Category { get; set; } = "Загальна";

    /// <summary>Одиниця виміру: «хв», «повторень», «км».</summary>
    public string Unit { get; set; } = "";

    /// <summary>Скільки балів навантаження дає одна одиниця виміру.</summary>
    public double LoadPerUnit { get; set; }

    public Exercise() { }

    public Exercise(int id, string name, string unit, double loadPerUnit)
    {
        Id = id;
        Name = name;
        Unit = unit;
        LoadPerUnit = loadPerUnit;
    }

    /// <summary>
    /// Обчислює навантаження за кількістю, яку ввів користувач (рядком).
    /// Повертає null, якщо рядок не є невід’ємним числом.
    /// </summary>
    public double? CalculateLoad(string given)
    {
        // Українською десяткову частину пишуть через кому, тож приймаємо і кому, і крапку.
        var text = given.Trim().Replace(',', '.');

        if (!double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var amount)
            || !double.IsFinite(amount)
            || amount < 0)
            return null;

        return amount * LoadPerUnit;
    }

    /// <summary>Чи збігається назва вправи з введеним текстом (без урахування регістру й пробілів по краях).</summary>
    public bool HasName(string text) =>
        string.Equals(text.Trim(), Name, StringComparison.CurrentCultureIgnoreCase);

    public override string ToString() => $"#{Id} [{Category}] {Name}, {Unit}";
}
