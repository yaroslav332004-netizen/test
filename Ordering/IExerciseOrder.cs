using TrainingDiary.Exercises;

namespace TrainingDiary.Ordering;

/// <summary>У якому порядку показувати вправи.</summary>
public interface IExerciseOrder
{
    string Name { get; }
    List<Exercise> Order(List<Exercise> exercises);
}
