using TrainingDiary.Exercises;

namespace TrainingDiary.Ordering;

/// <summary>Групує вправи за категорією (за абеткою), всередині категорії — за номером.</summary>
public class ByCategoryOrder : IExerciseOrder
{
    public string Name => "за категорією";

    public List<Exercise> Order(List<Exercise> exercises) =>
        exercises
            .OrderBy(e => e.Category, StringComparer.CurrentCultureIgnoreCase)
            .ThenBy(e => e.Id)
            .ToList();
}
