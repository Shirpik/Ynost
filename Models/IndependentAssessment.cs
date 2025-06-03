namespace Ynost.Models;

// Для таблицы "4. Результаты освоения обучающимися образовательных программ по итогам независимой оценки качества образования."
public record IndependentAssessment(
    string AssessmentName,      // Вид независимой оценки качества (организация, осуществляющая оценку) (было Procedure)
    DateTime? AssessmentDate,   // Дата (было Date)
    string? ClassSubject,       // Класс/Предмет
    int? StudentsTotal,         // обучающихся, всего (было Students)
    int? StudentsParticipated,  // обучающихся, принявших участие (было Participated)
    int? StudentsPassed,        // обучающихся, справившихся с работой (было Passed)
    double? PerformanceRate,    // Качество успеваемости (%) - новое
    double? LearningQualityRate,// Качество обученности (%) - новое (название примерное)
    double? SouRate,            // СОУ (%) - новое
    string? Link);