namespace TrainingDiary.Exercises;

/// <summary>Вправа на час: навантаження залежить від тривалості в хвилинах.</summary>
public class TimedExercise : Exercise
{
    /// <summary>Скільки балів дає одна хвилина.</summary>
    public double PointsPerMinute { get; set; }

    /// <summary>Найбільша правдоподібна тривалість, хв. Більше — вважаємо помилкою введення.</summary>
    public double MaxMinutes { get; set; } = 600;

    public TimedExercise() { }

    public TimedExercise(int id, string name, double pointsPerMinute)
        : base(id, name)
    {
        PointsPerMinute = pointsPerMinute;
    }

    public TimedExercise(int id, string name, double pointsPerMinute, double maxMinutes)
        : this(id, name, pointsPerMinute)
    {
        MaxMinutes = maxMinutes;
    }

    public override double? CalculateLoad(string given)
    {
        if (!TryParseNumber(given, out var minutes) || minutes < 0 || minutes > MaxMinutes)
            return null;

        return minutes * PointsPerMinute;
    }

    public override string LoadRule => $"{PointsPerMinute:0.##} б. за хвилину (до {MaxMinutes:0.##} хв)";

    public override string Prompt => "Тривалість, хв";

    public override string Kind => "timed";

    public override void WriteData(IDictionary<string, string> data)
    {
        data["pointsPerMinute"] = WriteNumber(PointsPerMinute);
        data["maxMinutes"] = WriteNumber(MaxMinutes);
    }

    public override void ReadData(IReadOnlyDictionary<string, string> data)
    {
        PointsPerMinute = ReadNumber(data, "pointsPerMinute");
        MaxMinutes = ReadNumber(data, "maxMinutes", MaxMinutes);
    }
}
