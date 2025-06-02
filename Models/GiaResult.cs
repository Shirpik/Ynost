namespace Ynost.Models;

// Для таблицы "2. Результаты государственной итоговой аттестации (ГИА)"
public record GiaResult(
    string Subject,
    string Class,
    int? TotalParticipants,
    double? PctMark5, // % «5»
    double? PctMark4, // % «4»
    double? PctMark3, // % «3»
    double? PctMark2, // % «2» (если нужно)
    double? PctFail,  // % не преодолевших мин. балл
    double? AvgScore, // Средний балл
    string? Link);