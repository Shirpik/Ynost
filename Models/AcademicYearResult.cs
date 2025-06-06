using System;

namespace Ynost.Models;

public class AcademicYearResult
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TeacherId { get; set; }        // ← добавить
    public string Group { get; set; } = string.Empty;
    public string AcademicPeriod { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string AvgSem1 { get; set; } = string.Empty;
    public string AvgSem2 { get; set; } = string.Empty;
    public string Dynamics { get; set; } = string.Empty;
    public string AvgSuccessRate { get; set; } = string.Empty;
    public string AvgQualityRate { get; set; } = string.Empty;
    public string SouRate { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;
}
