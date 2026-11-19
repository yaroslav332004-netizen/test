using TrainingDiary.Exercises;

namespace TrainingDiary.Storage;

internal static class ExerciseFactory
{
    public static Exercise? Create(string kind) => kind switch
    {
        "timed" => new TimedExercise(),
        "reps" => new RepsExercise(),
        "distance" => new DistanceExercise(),
        _ => null
    };

    public static Exercise? FromRecord(ExerciseRecord record)
    {
        var exercise = Create(record.Kind);

        if (exercise is null)
            return null;

        exercise.Id = record.Id;
        exercise.Name = record.Name;
        exercise.Category = record.Category;
        exercise.ReadData(record.Data);

        return exercise;
    }

    public static ExerciseRecord ToRecord(Exercise exercise)
    {
        var record = new ExerciseRecord
        {
            Id = exercise.Id,
            Kind = exercise.Kind,
            Name = exercise.Name,
            Category = exercise.Category
        };

        exercise.WriteData(record.Data);

        return record;
    }
}
