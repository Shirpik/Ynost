namespace Ynost.Models;

public record IndependentAssessment(
    string? AssessmentName,
    string? AssessmentDateString,   // Было DateTime? Date, формат "dd.MM.yyyy"
    string? ClassSubject,
    string? StudentsTotal,         // Было int?
    string? StudentsParticipated,
    string? StudentsPassed,
    string? PerformanceRate,
    string? LearningQualityRate,
    string? SouRate,
    string? Link);