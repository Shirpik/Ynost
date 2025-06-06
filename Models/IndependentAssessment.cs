using System;

namespace Ynost.Models;

public class IndependentAssessment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TeacherId { get; set; }        // ← добавить
    public string AssessmentName { get; set; } = string.Empty;
    public string AssessmentDate { get; set; } = string.Empty;   // храните как текст
    public string ClassSubject { get; set; } = string.Empty;
    public string StudentsTotal { get; set; } = string.Empty;
    public string StudentsParticipated { get; set; } = string.Empty;
    public string StudentsPassed { get; set; } = string.Empty;
    public string PerformanceRate { get; set; } = string.Empty;
    public string LearningQualityRate { get; set; } = string.Empty;
    public string SouRate { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;
}
