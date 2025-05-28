namespace Ynost.Models;
public record IndependentAssessment(
    string Procedure,
    DateTime Date,
    string ClassSubject,
    int Students,
    int Participated,
    int Passed,
    string? Link);