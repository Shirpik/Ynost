namespace Ynost.Models;
public record Mentorship(
    string? Trainee,
    string? Order,
    string? OrderDateString, // Было DateTime OrderDate
    string? Link);