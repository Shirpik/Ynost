namespace Ynost.Models;
public record EgeResult(
    string Subject,
    string Class,
    int Total,
    double Pct81to99,
    double Pct61to80,
    double PctMinTo60,
    double PctFail,
    string? Link);