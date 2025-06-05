namespace Ynost.Models;

public record DemoExamResult(
    string? Subject,
    string? Group,
    string? TotalParticipants, // Было int?
    string? PctMark5,          // Было double?
    string? PctMark4,
    string? PctMark3,
    string? PctMark2,
    string? AvgScore,
    string? Link);