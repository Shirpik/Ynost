namespace Ynost.Models;
public record TrainingCourse(
    string CourseName,
    int Hours,
    int Year,
    string Provider,
    string? CertificateLink);