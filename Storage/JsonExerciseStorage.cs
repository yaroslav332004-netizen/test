using System.Text.Encodings.Web;
using System.Text.Json;
using TrainingDiary.Exercises;

namespace TrainingDiary.Storage;

public class JsonExerciseStorage : IExerciseStorage
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        // Без цього кирилиця у файлі перетвориться на \u0410\u0411…
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    private readonly string path;

    public JsonExerciseStorage(string path) => this.path = path;

    public string Path => path;

    public async Task<List<Exercise>> LoadAsync()
    {
        if (!File.Exists(path))
            return new List<Exercise>();

        string json;

        try
        {
            json = await File.ReadAllTextAsync(path);
        }
        catch (Exception ex)
            when (ex is IOException or UnauthorizedAccessException)
        {
            throw new ExerciseStorageException(
                $"Не вдалося прочитати файл {path}.", ex);
        }

        List<ExerciseRecord>? records;

        try
        {
            records = JsonSerializer.Deserialize<List<ExerciseRecord>>(json, Options);
        }
        catch (JsonException ex)
        {
            throw new ExerciseStorageException(
                $"Файл {path} пошкоджено: це не коректний JSON.", ex);
        }

        var exercises = new List<Exercise>();

        foreach (var record in records ?? new List<ExerciseRecord>())
        {
            var exercise = record is null ? null : ExerciseFactory.FromRecord(record);

            // Запис невідомого різновиду пропускають, решту читають.
            if (exercise is not null)
                exercises.Add(exercise);
        }

        return exercises;
    }

    public async Task SaveAsync(List<Exercise> exercises)
    {
        var records = new List<ExerciseRecord>();

        foreach (var exercise in exercises)
            records.Add(ExerciseFactory.ToRecord(exercise));

        var json = JsonSerializer.Serialize(records, Options);

        try
        {
            // System.IO.Path повністю: у класі є власна властивість Path.
            var folder = System.IO.Path.GetDirectoryName(path);

            if (!string.IsNullOrEmpty(folder))
                Directory.CreateDirectory(folder);

            await File.WriteAllTextAsync(path, json);
        }
        catch (Exception ex)
            when (ex is IOException or UnauthorizedAccessException)
        {
            throw new ExerciseStorageException(
                $"Не вдалося записати файл {path}.", ex);
        }
    }
}
