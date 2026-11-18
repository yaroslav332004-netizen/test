namespace TrainingDiary.Exercises;

/// <summary>Вправа на дистанцію: навантаження залежить від кілометрів, довга дистанція дає бонус.</summary>
public class DistanceExercise : Exercise
{
    /// <summary>Бонус за кожен кілометр понад межу — половина звичайної ціни кілометра.</summary>
    private const double BonusShare = 0.5;

    /// <summary>Скільки балів дає один кілометр.</summary>
    public double PointsPerKm { get; set; }

    /// <summary>Після скількох кілометрів починається бонус. Нуль — бонусу немає.</summary>
    public double BonusFromKm { get; set; }

    public DistanceExercise() { }

    public DistanceExercise(int id, string name, double pointsPerKm)
        : base(id, name)
    {
        PointsPerKm = pointsPerKm;
    }

    public DistanceExercise(int id, string name, double pointsPerKm, double bonusFromKm)
        : this(id, name, pointsPerKm)
    {
        BonusFromKm = bonusFromKm;
    }

    public override double? CalculateLoad(string given)
    {
        if (!TryParseNumber(given, out var km) || km < 0)
            return null;

        var load = km * PointsPerKm;

        if (BonusFromKm > 0 && km > BonusFromKm)
            load += (km - BonusFromKm) * PointsPerKm * BonusShare;

        return load;
    }

    public override string LoadRule =>
        BonusFromKm > 0
            ? $"{PointsPerKm:0.##} б. за км, понад {BonusFromKm:0.##} км ще +{BonusShare:P0}"
            : $"{PointsPerKm:0.##} б. за км";

    public override string Prompt => "Дистанція, км";

    public override string Kind => "distance";

    public override void WriteData(IDictionary<string, string> data)
    {
        data["pointsPerKm"] = WriteNumber(PointsPerKm);
        data["bonusFromKm"] = WriteNumber(BonusFromKm);
    }

    public override void ReadData(IReadOnlyDictionary<string, string> data)
    {
        PointsPerKm = ReadNumber(data, "pointsPerKm");
        BonusFromKm = ReadNumber(data, "bonusFromKm");
    }
}
