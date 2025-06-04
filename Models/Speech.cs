namespace Ynost.Models;
public record Speech(
    string? Level,
    string? Name,
    string? DateString, // Было DateTime Date
    string? Link);