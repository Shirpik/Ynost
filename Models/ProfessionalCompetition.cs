namespace Ynost.Models;
public record ProfessionalCompetition(
    string? Level,
    string? Name,
    string? Achievement,
    string? DateString, // Было DateTime Date
    string? Link);