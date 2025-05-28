namespace Ynost.Models;
public record AcademicYearResult(
    int Year,
    string Subject,
    double? AvgSem1,
    double? AvgSem2,
    double? Dynamics,
    double? AvgSuccess,
    double? AvgQuality,
    double? Sou,
    string? Link);