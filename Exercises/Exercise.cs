using System.Globalization;

namespace TrainingDiary.Exercises;

/// <summary>Спільна основа всіх вправ: те, що є в кожної.</summary>
public abstract class Exercise
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Category { get; set; } = "Загальна";

    protected Exercise() { }

    protected Exercise(int id, string name)
    {
        Id = id;
        Name = name;
    }

    /// <summary>
    /// Обчислює навантаження за кількістю, яку ввів користувач (рядком).
    /// Кожен різновид — по-своєму. Повертає null, якщо введення некоректне.
    /// </summary>
    public abstract double? CalculateLoad(string given);

    /// <summary>Як рахується навантаження, текстом — для показу.</summary>
    public abstract string LoadRule { get; }

    /// <summary>Підказка, що вводити. Різновид може змінити.</summary>
    public virtual string Prompt => "Кількість";

    /// <summary>
    /// Читає число за налаштуваннями системи, а якщо не вийшло — за незмінними правилами.
    /// Нескінченності й NaN не приймаються.
    /// </summary>
    protected static bool TryParseNumber(string text, out double value)
    {
        var trimmed = text.Trim();

        var ok = double.TryParse(trimmed, NumberStyles.Float,
                     CultureInfo.CurrentCulture, out value)
              || double.TryParse(trimmed, NumberStyles.Float,
                     CultureInfo.InvariantCulture, out value);

        return ok && double.IsFinite(value);
    }

    public override string ToString() => $"#{Id} [{Category}] {Name}";
}
