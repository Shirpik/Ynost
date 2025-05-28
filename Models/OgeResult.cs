namespace Ynost.Models;
public record OgeResult(
    string Subject,
    string Class,
    int Total,
    int Mark5,
    int Mark4,
    int Mark3,
    int Mark2,
    double Avg,
    string? Link);