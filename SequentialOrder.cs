using TrainingDiary.Exercises;

namespace TrainingDiary.Ordering;

/// <summary>Показує вправи в тому порядку, у якому вони лежать.</summary>
public class SequentialOrder : IExerciseOrder
{
    public string Name => "як записано";

    public List<Exercise> Order(List<Exercise> exercises) => new(exercises);
}
