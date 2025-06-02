namespace Ynost.Models;

// Для таблицы "1. Учебный год" (Итоговые результаты успеваемости)
public record AcademicYearResult(
    int Year,
    string Subject,
    double? AvgSem1,
    double? AvgSem2,
    double? Dynamics,
    double? AvgSuccessRate, // Средний балл успеваемости по предмету (было AvgSuccess)
    double? AvgQualityRate, // Средний балл качества обучения по предмету (было AvgQuality)
    double? SouRate,        // СОУ по предмету (было Sou)
    string? Link);