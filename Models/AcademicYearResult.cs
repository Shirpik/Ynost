namespace Ynost.Models;

public record AcademicYearResult(
    string? Group,
    string? AcademicPeriod, // Было int Year, формат "2024-2025"
    string? Subject,
    string? AvgSem1,        // Было double?
    string? AvgSem2,        // Было double?
    string? Dynamics,       // Было double?
    string? AvgSuccessRate, // Было double?
    string? AvgQualityRate, // Было double?
    string? SouRate,        // Было double?
    string? Link);