
# TrainingDiary — Щоденник тренувань

Тема проєкту: **«Щоденник тренувань»** (тема №16 з переліку). Застосунок веде облік вправ трьох видів — на час, на повторення й на дистанцію — і для кожної обчислює навантаження.

Це навчальний проєкт з дисципліни «Крос-платформне програмування» (ЖДУ імені Івана Франка). Він збирається й запускається у Windows, macOS і Linux.

## Структура

- `TrainingDiary.Core/` — бібліотека класів: вправи, порядок перебору, сховище, ядро сеансу. Без жодного коду для виведення.
- `TrainingDiary.App/` — консольна оболонка на Spectre.Console.
- `tools/DiaryReport/` — другий застосунок, який підключає бібліотеку пакетом NuGet, а не проєктом.

## Як запустити

Потрібен .NET SDK 10.

    dotnet run --project TrainingDiary.App

## Пакет бібліотеки

    dotnet pack TrainingDiary.Core -c Release -o ./packages
    dotnet run --project tools/DiaryReport -- TrainingDiary.App/bin/Debug/net10.0/data/diary.json

Локальне джерело пакетів описано в `nuget.config`. Після зміни бібліотеки збільшіть `Version` у `TrainingDiary.Core.csproj`, запакуйте знову й оновіть версію в `tools/DiaryReport/DiaryReport.csproj`.

## Карта назв

| Роль у кістяку | У прикладі (CardTrainer) | Мій варіант (TrainingDiary) |
|---|---|---|
| Основний тип | Card | Exercise |
| Різновид 1 | OpenAnswerCard | TimedExercise (на час) |
| Різновид 2 | ChoiceCard | RepsExercise (на повторення) |
| Різновид 3 | NumericCard | DistanceExercise (на дистанцію) |
| Спільна дія | Check(given) | CalculateLoad(amount) |
| Набір записів | List<Card> deck | List<Exercise> exercises |
| Сховище | ICardStorage | IExerciseStorage |
| Порядок перебору | IReviewStrategy | IExerciseOrder |
| Підсумок | SessionSummary | WorkoutSummary |
| Різновид 4 (ЛР 8) | TrueFalseCard | IntervalExercise (на інтервали) |
