namespace Ynost.Models;

// Для таблицы "3. Результаты государственной аттестации (ДЭ)"
public record DemoExamResult(
    string Subject, // В Word "Предмет", но таблица называется "Наименование компетенции"
    string Group, // В Word "Группа"
    int? TotalParticipants,
    double? PctMark5, // % «5»
    double? PctMark4, // % «4»
    double? PctMark3, // % «3»
    double? PctMark2, // % «2»
    double? AvgScore, // Средний балл
    string? Link);