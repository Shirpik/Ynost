using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ynost.Models;

namespace Ynost.ViewModels
{
    public partial class TeacherViewModel : ObservableObject
    {
        private readonly Teacher _model;

        public TeacherViewModel(Teacher model)
        {
            _model = model;
            FullName = model.FullName;

            AcademicResults = new ObservableCollection<AcademicYearResult>(model.AcademicResults);
            EgeResults = new ObservableCollection<EgeResult>(model.EgeResults);
            OgeResults = new ObservableCollection<OgeResult>(model.OgeResults);
            IndependentAssessments = new ObservableCollection<IndependentAssessment>(model.IndependentAssessments);
            SelfDeterminations = new ObservableCollection<SelfDeterminationActivity>(model.SelfDeterminations);
            StudentOlympiads = new ObservableCollection<StudentOlympiad>(model.StudentOlympiads);
            JuryActivities = new ObservableCollection<JuryActivity>(model.JuryActivities);
            MasterClasses = new ObservableCollection<MasterClass>(model.MasterClasses);
            Speeches = new ObservableCollection<Speech>(model.Speeches);
            Publications = new ObservableCollection<Publication>(model.Publications);
            Trainings = new ObservableCollection<TrainingCourse>(model.Trainings);
            ExperimentalProjects = new ObservableCollection<ExperimentalProject>(model.ExperimentalProjects);
            Mentorships = new ObservableCollection<Mentorship>(model.Mentorships);
            ProgramSupports = new ObservableCollection<ProgramMethodSupport>(model.ProgramSupports);
            ProfessionalCompetitions = new ObservableCollection<ProfessionalCompetition>(model.ProfessionalCompetitions);
        }

        public string FullName { get; }

        public int GetModelId()
        {
            return _model.Id;
        }

        // 1. AcademicYearResult
        public ObservableCollection<AcademicYearResult> AcademicResults { get; }
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteAcademicResultCommand))]
        private AcademicYearResult? selectedAcademicResult;

        [RelayCommand]
        private void AddAcademicResult()
        {
            var newItem = new AcademicYearResult(DateTime.Now.Year, "Новый предмет", null, null, null, null, null, null, null);
            AcademicResults.Add(newItem);
            // SelectedAcademicResult = newItem; // Убрали автоматический выбор
        }
        [RelayCommand(CanExecute = nameof(CanDeleteAcademicResult))]
        private void DeleteAcademicResult()
        {
            if (SelectedAcademicResult != null)
            {
                AcademicResults.Remove(SelectedAcademicResult);
                SelectedAcademicResult = null;
            }
        }
        private bool CanDeleteAcademicResult() => SelectedAcademicResult != null;

        // 2. EgeResult
        public ObservableCollection<EgeResult> EgeResults { get; }
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteEgeResultCommand))]
        private EgeResult? selectedEgeResult;

        [RelayCommand]
        private void AddEgeResult()
        {
            var newItem = new EgeResult("Новый предмет", "Класс", 0, 0, 0, 0, 0, null);
            EgeResults.Add(newItem);
            // SelectedEgeResult = newItem; // Убрали автоматический выбор
        }
        [RelayCommand(CanExecute = nameof(CanDeleteEgeResult))]
        private void DeleteEgeResult()
        {
            if (SelectedEgeResult != null)
            {
                EgeResults.Remove(SelectedEgeResult);
                SelectedEgeResult = null;
            }
        }
        private bool CanDeleteEgeResult() => SelectedEgeResult != null;

        // 3. OgeResult
        public ObservableCollection<OgeResult> OgeResults { get; }
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteOgeResultCommand))]
        private OgeResult? selectedOgeResult;

        [RelayCommand]
        private void AddOgeResult()
        {
            var newItem = new OgeResult("Новый предмет", "Класс", 0, 0, 0, 0, 0, 0, null);
            OgeResults.Add(newItem);
            // SelectedOgeResult = newItem; // Убрали автоматический выбор
        }
        [RelayCommand(CanExecute = nameof(CanDeleteOgeResult))]
        private void DeleteOgeResult()
        {
            if (SelectedOgeResult != null)
            {
                OgeResults.Remove(SelectedOgeResult);
                SelectedOgeResult = null;
            }
        }
        private bool CanDeleteOgeResult() => SelectedOgeResult != null;

        // 4. IndependentAssessment
        public ObservableCollection<IndependentAssessment> IndependentAssessments { get; }
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteIndependentAssessmentCommand))]
        private IndependentAssessment? selectedIndependentAssessment;

        [RelayCommand]
        private void AddIndependentAssessment()
        {
            var newItem = new IndependentAssessment("Процедура", DateTime.Now, "Класс/Предмет", 0, 0, 0, null);
            IndependentAssessments.Add(newItem);
            // SelectedIndependentAssessment = newItem; // Убрали автоматический выбор
        }
        [RelayCommand(CanExecute = nameof(CanDeleteIndependentAssessment))]
        private void DeleteIndependentAssessment()
        {
            if (SelectedIndependentAssessment != null)
            {
                IndependentAssessments.Remove(SelectedIndependentAssessment);
                SelectedIndependentAssessment = null;
            }
        }
        private bool CanDeleteIndependentAssessment() => SelectedIndependentAssessment != null;

        // 5. SelfDeterminationActivity
        public ObservableCollection<SelfDeterminationActivity> SelfDeterminations { get; }
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteSelfDeterminationCommand))]
        private SelfDeterminationActivity? selectedSelfDetermination;

        [RelayCommand]
        private void AddSelfDetermination()
        {
            var newItem = new SelfDeterminationActivity("Уровень", "Мероприятие", "Роль", null);
            SelfDeterminations.Add(newItem);
            // SelectedSelfDetermination = newItem; // Убрали автоматический выбор
        }
        [RelayCommand(CanExecute = nameof(CanDeleteSelfDetermination))]
        private void DeleteSelfDetermination()
        {
            if (SelectedSelfDetermination != null)
            {
                SelfDeterminations.Remove(SelectedSelfDetermination);
                SelectedSelfDetermination = null;
            }
        }
        private bool CanDeleteSelfDetermination() => SelectedSelfDetermination != null;

        // 6. StudentOlympiad
        public ObservableCollection<StudentOlympiad> StudentOlympiads { get; }
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteStudentOlympiadCommand))]
        private StudentOlympiad? selectedStudentOlympiad;

        [RelayCommand]
        private void AddStudentOlympiad()
        {
            var newItem = new StudentOlympiad("Уровень", "Название", "Форма", "Ученик", "Результат", null);
            StudentOlympiads.Add(newItem);
        }
        [RelayCommand(CanExecute = nameof(CanDeleteStudentOlympiad))]
        private void DeleteStudentOlympiad()
        {
            if (SelectedStudentOlympiad != null)
            {
                StudentOlympiads.Remove(SelectedStudentOlympiad);
                SelectedStudentOlympiad = null;
            }
        }
        private bool CanDeleteStudentOlympiad() => SelectedStudentOlympiad != null;

        // 7. JuryActivity
        public ObservableCollection<JuryActivity> JuryActivities { get; }
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteJuryActivityCommand))]
        private JuryActivity? selectedJuryActivity;

        [RelayCommand]
        private void AddJuryActivity()
        {
            var newItem = new JuryActivity("Уровень", "Событие", DateTime.Now, null);
            JuryActivities.Add(newItem);
        }
        [RelayCommand(CanExecute = nameof(CanDeleteJuryActivity))]
        private void DeleteJuryActivity()
        {
            if (SelectedJuryActivity != null)
            {
                JuryActivities.Remove(SelectedJuryActivity);
                SelectedJuryActivity = null;
            }
        }
        private bool CanDeleteJuryActivity() => SelectedJuryActivity != null;

        // 8. MasterClass
        public ObservableCollection<MasterClass> MasterClasses { get; }
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteMasterClassCommand))]
        private MasterClass? selectedMasterClass;

        [RelayCommand]
        private void AddMasterClass()
        {
            var newItem = new MasterClass("Уровень", "Тема", DateTime.Now, null);
            MasterClasses.Add(newItem);
        }
        [RelayCommand(CanExecute = nameof(CanDeleteMasterClass))]
        private void DeleteMasterClass()
        {
            if (SelectedMasterClass != null)
            {
                MasterClasses.Remove(SelectedMasterClass);
                SelectedMasterClass = null;
            }
        }
        private bool CanDeleteMasterClass() => SelectedMasterClass != null;

        // 9. Speech
        public ObservableCollection<Speech> Speeches { get; }
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteSpeechCommand))]
        private Speech? selectedSpeech;

        [RelayCommand]
        private void AddSpeech()
        {
            var newItem = new Speech("Уровень", "Тема", DateTime.Now, null);
            Speeches.Add(newItem);
        }
        [RelayCommand(CanExecute = nameof(CanDeleteSpeech))]
        private void DeleteSpeech()
        {
            if (SelectedSpeech != null)
            {
                Speeches.Remove(SelectedSpeech);
                SelectedSpeech = null;
            }
        }
        private bool CanDeleteSpeech() => SelectedSpeech != null;

        // 10. Publication
        public ObservableCollection<Publication> Publications { get; }
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeletePublicationCommand))]
        private Publication? selectedPublication;

        [RelayCommand]
        private void AddPublication()
        {
            var newItem = new Publication("Уровень", "Название", DateTime.Now, null);
            Publications.Add(newItem);
        }
        [RelayCommand(CanExecute = nameof(CanDeletePublication))]
        private void DeletePublication()
        {
            if (SelectedPublication != null)
            {
                Publications.Remove(SelectedPublication);
                SelectedPublication = null;
            }
        }
        private bool CanDeletePublication() => SelectedPublication != null;

        // 11. TrainingCourse
        public ObservableCollection<TrainingCourse> Trainings { get; }
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteTrainingCourseCommand))]
        private TrainingCourse? selectedTrainingCourse;

        [RelayCommand]
        private void AddTrainingCourse()
        {
            var newItem = new TrainingCourse("Название курса", 0, DateTime.Now.Year, "Организация", null);
            Trainings.Add(newItem);
        }
        [RelayCommand(CanExecute = nameof(CanDeleteTrainingCourse))]
        private void DeleteTrainingCourse()
        {
            if (SelectedTrainingCourse != null)
            {
                Trainings.Remove(SelectedTrainingCourse);
                SelectedTrainingCourse = null;
            }
        }
        private bool CanDeleteTrainingCourse() => SelectedTrainingCourse != null;

        // 12. ExperimentalProject
        public ObservableCollection<ExperimentalProject> ExperimentalProjects { get; }
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteExperimentalProjectCommand))]
        private ExperimentalProject? selectedExperimentalProject;

        [RelayCommand]
        private void AddExperimentalProject()
        {
            var newItem = new ExperimentalProject("Название проекта", DateTime.Now, null);
            ExperimentalProjects.Add(newItem);
        }
        [RelayCommand(CanExecute = nameof(CanDeleteExperimentalProject))]
        private void DeleteExperimentalProject()
        {
            if (SelectedExperimentalProject != null)
            {
                ExperimentalProjects.Remove(SelectedExperimentalProject);
                SelectedExperimentalProject = null;
            }
        }
        private bool CanDeleteExperimentalProject() => SelectedExperimentalProject != null;

        // 13. Mentorship
        public ObservableCollection<Mentorship> Mentorships { get; }
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteMentorshipCommand))]
        private Mentorship? selectedMentorship;

        [RelayCommand]
        private void AddMentorship()
        {
            var newItem = new Mentorship("Наставляемый", "Приказ", DateTime.Now, null);
            Mentorships.Add(newItem);
        }
        [RelayCommand(CanExecute = nameof(CanDeleteMentorship))]
        private void DeleteMentorship()
        {
            if (SelectedMentorship != null)
            {
                Mentorships.Remove(SelectedMentorship);
                SelectedMentorship = null;
            }
        }
        private bool CanDeleteMentorship() => SelectedMentorship != null;

        // 14. ProgramMethodSupport
        public ObservableCollection<ProgramMethodSupport> ProgramSupports { get; }
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteProgramSupportCommand))]
        private ProgramMethodSupport? selectedProgramSupport;

        [RelayCommand]
        private void AddProgramSupport()
        {
            var newItem = new ProgramMethodSupport("Название программы", false, null);
            ProgramSupports.Add(newItem);
        }
        [RelayCommand(CanExecute = nameof(CanDeleteProgramSupport))]
        private void DeleteProgramSupport()
        {
            if (SelectedProgramSupport != null)
            {
                ProgramSupports.Remove(SelectedProgramSupport);
                SelectedProgramSupport = null;
            }
        }
        private bool CanDeleteProgramSupport() => SelectedProgramSupport != null;

        // 15. ProfessionalCompetition
        public ObservableCollection<ProfessionalCompetition> ProfessionalCompetitions { get; }
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteProfessionalCompetitionCommand))]
        private ProfessionalCompetition? selectedProfessionalCompetition;

        [RelayCommand]
        private void AddProfessionalCompetition()
        {
            var newItem = new ProfessionalCompetition("Уровень", "Конкурс", "Достижение", DateTime.Now, null);
            ProfessionalCompetitions.Add(newItem);
        }
        [RelayCommand(CanExecute = nameof(CanDeleteProfessionalCompetition))]
        private void DeleteProfessionalCompetition()
        {
            if (SelectedProfessionalCompetition != null)
            {
                ProfessionalCompetitions.Remove(SelectedProfessionalCompetition);
                SelectedProfessionalCompetition = null;
            }
        }
        private bool CanDeleteProfessionalCompetition() => SelectedProfessionalCompetition != null;
    }
}