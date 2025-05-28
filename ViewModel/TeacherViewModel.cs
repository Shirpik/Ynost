using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Ynost.Models;
namespace Ynost.ViewModels;
public partial class TeacherViewModel : ObservableObject
{
    public TeacherViewModel(Teacher model)
    {
        _model = model;
        FullName = model.FullName;
        AcademicResults = new(model.AcademicResults);
        EgeResults = new(model.EgeResults);
        OgeResults = new(model.OgeResults);
        IndependentAssessments = new(model.IndependentAssessments);
        SelfDeterminations = new(model.SelfDeterminations);
        StudentOlympiads = new(model.StudentOlympiads);
        JuryActivities = new(model.JuryActivities);
        MasterClasses = new(model.MasterClasses);
        Speeches = new(model.Speeches);
        Publications = new(model.Publications);
        Trainings = new(model.Trainings);
        ExperimentalProjects = new(model.ExperimentalProjects);
        Mentorships = new(model.Mentorships);
        ProgramSupports = new(model.ProgramSupports);
        ProfessionalCompetitions = new(model.ProfessionalCompetitions);
    }
    private readonly Teacher _model;
    [ObservableProperty] private string fullName = string.Empty;
    public ObservableCollection<AcademicYearResult> AcademicResults { get; }
    public ObservableCollection<EgeResult> EgeResults { get; }
    public ObservableCollection<OgeResult> OgeResults { get; }
    public ObservableCollection<IndependentAssessment> IndependentAssessments { get; }
    public ObservableCollection<SelfDeterminationActivity> SelfDeterminations { get; }
    public ObservableCollection<StudentOlympiad> StudentOlympiads { get; }
    public ObservableCollection<JuryActivity> JuryActivities { get; }
    public ObservableCollection<MasterClass> MasterClasses { get; }
    public ObservableCollection<Speech> Speeches { get; }
    public ObservableCollection<Publication> Publications { get; }
    public ObservableCollection<TrainingCourse> Trainings { get; }
    public ObservableCollection<ExperimentalProject> ExperimentalProjects { get; }
    public ObservableCollection<Mentorship> Mentorships { get; }
    public ObservableCollection<ProgramMethodSupport> ProgramSupports { get; }
    public ObservableCollection<ProfessionalCompetition> ProfessionalCompetitions { get; }
}