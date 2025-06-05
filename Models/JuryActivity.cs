namespace Ynost.Models;
public record JuryActivity(
    string? Level,
    string? Name,
    string? DateString, // Было DateTime Date
    string? Link);