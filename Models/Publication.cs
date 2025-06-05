namespace Ynost.Models;
public record Publication(
    string? Level,
    string? Title,
    string? DateString, // Было DateTime Date
    string? Link);