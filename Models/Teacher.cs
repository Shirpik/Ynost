namespace Ynost.Models;

public class Teacher
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public bool IsLecturer { get; set; } // Это поле было в DatabaseService, добавим его сюда, если оно нужно

    public List<AcademicYearResult> AcademicResults { get; } = new();
    public List<GiaResult> GiaResults { get; } = new(); // Заменили ЕГЭ/ОГЭ
    public List<DemoExamResult> DemoExamResults { get; } = new(); // Новая таблица ДЭ
    public List<IndependentAssessment> IndependentAssessments { get; } = new();
    public List<SelfDeterminationActivity> SelfDeterminations { get; } = new();
    public List<StudentOlympiad> StudentOlympiads { get; } = new();
    public List<JuryActivity> JuryActivities { get; } = new();
    public List<MasterClass> MasterClasses { get; } = new();
    public List<Speech> Speeches { get; } = new();
    public List<Publication> Publications { get; } = new();
    // Trainings УДАЛЕНЫ
    public List<ExperimentalProject> ExperimentalProjects { get; } = new();
    public List<Mentorship> Mentorships { get; } = new();
    public List<ProgramMethodSupport> ProgramSupports { get; } = new();
    public List<ProfessionalCompetition> ProfessionalCompetitions { get; } = new();
}