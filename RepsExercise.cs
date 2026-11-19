using System.Globalization;

namespace TrainingDiary.Exercises;

/// <summary>Вправа на повторення: навантаження залежить від кількості повторень і складності варіанта.</summary>
public class RepsExercise : Exercise
{
    /// <summary>Скільки балів дає одне повторення.</summary>
    public double PointsPerRep { get; set; }

    /// <summary>Варіанти виконання від простого до складного.</summary>
    public List<string> Variants { get; set; } = new() { "Звичайні" };

    /// <summary>Номер обраного варіанта, починаючи з одиниці.</summary>
    public int Variant { get; set; } = 1;

    /// <summary>Варіант виконання за номером, починаючи з одиниці.</summary>
    public string this[int number] => Variants[number - 1];

    /// <summary>Номер варіанта, який справді існує (інакше — перший).</summary>
    private int ValidVariant => Variant >= 1 && Variant <= Variants.Count ? Variant : 1;

    /// <summary>Кожен наступний варіант складніший на чверть.</summary>
    private double VariantFactor => 1 + 0.25 * (ValidVariant - 1);

    public RepsExercise() { }

    public RepsExercise(int id, string name, double pointsPerRep)
        : base(id, name)
    {
        PointsPerRep = pointsPerRep;
    }

    public RepsExercise(int id, string name, double pointsPerRep,
                        int variant, params string[] variants)
        : this(id, name, pointsPerRep)
    {
        Variant = variant;

        if (variants.Length > 0)
        {
            Variants.Clear();
            Variants.AddRange(variants);
        }
    }

    public override double? CalculateLoad(string given)
    {
        // Повторень має бути ціле невід’ємне число.
        if (!TryParseNumber(given, out var reps) || reps < 0 || reps != Math.Floor(reps))
            return null;

        return reps * PointsPerRep * VariantFactor;
    }

    public override string LoadRule =>
        $"{PointsPerRep:0.##} б. за повторення, варіант «{this[ValidVariant]}» ×{VariantFactor:0.##}";

    public override string Prompt => "Кількість повторень";

    public override string Kind => "reps";

    public override void WriteData(IDictionary<string, string> data)
    {
        // Роздільник ";" не може трапитися всередині значення:
        // інакше під час читання варіантів стане більше, ніж було.
        foreach (var variant in Variants)
            if (variant.Contains(';'))
                throw new InvalidOperationException(
                    $"Назва варіанта «{variant}» містить ';'.");

        data["pointsPerRep"] = WriteNumber(PointsPerRep);
        data["variant"] = Variant.ToString(CultureInfo.InvariantCulture);
        data["variants"] = string.Join(";", Variants);
    }

    public override void ReadData(IReadOnlyDictionary<string, string> data)
    {
        PointsPerRep = ReadNumber(data, "pointsPerRep");
        Variant = (int)ReadNumber(data, "variant", 1);

        if (data.TryGetValue("variants", out var text))
        {
            var variants = text.Split(';', StringSplitOptions.RemoveEmptyEntries);

            // Порожній перелік не підставляємо: лишаються варіанти за замовчуванням.
            if (variants.Length > 0)
            {
                Variants.Clear();
                Variants.AddRange(variants);
            }
        }
    }
}
