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

    /// <summary>Позначка різновиду: під нею вправу пишуть у файл.</summary>
    public abstract string Kind { get; }

    /// <summary>Записує власні дані різновиду до словника.</summary>
    public abstract void WriteData(IDictionary<string, string> data);

    /// <summary>Читає власні дані різновиду зі словника.</summary>
    public abstract void ReadData(IReadOnlyDictionary<string, string> data);

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

    /// <summary>
    /// Число для файлу: завжди за незмінними правилами (з крапкою), хоч би яка мова в системі.
    /// </summary>
    protected static string WriteNumber(double value) =>
        value.ToString(CultureInfo.InvariantCulture);

    /// <summary>
    /// Число з файлу: лише за незмінними правилами. Якщо ключа немає або там не число,
    /// повертає запасне значення.
    /// </summary>
    protected static double ReadNumber(
        IReadOnlyDictionary<string, string> data, string key, double fallback = 0) =>
        data.TryGetValue(key, out var text)
            && double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var value)
            && double.IsFinite(value)
            ? value
            : fallback;

    public override string ToString() => $"#{Id} [{Category}] {Name}";
}
