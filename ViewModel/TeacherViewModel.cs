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
            GiaResults = new ObservableCollection<GiaResult>(model.GiaResults); // Новое
            DemoExamResults = new ObservableCollection<DemoExamResult>(model.DemoExamResults); // Новое
            IndependentAssessments = new ObservableCollection<IndependentAssessment>(model.IndependentAssessments);
            SelfDeterminations = new ObservableCollection<SelfDeterminationActivity>(model.SelfDeterminations);
            StudentOlympiads = new ObservableCollection<StudentOlympiad>(model.StudentOlympiads);
            JuryActivities = new ObservableCollection<JuryActivity>(model.JuryActivities);
            MasterClasses = new ObservableCollection<MasterClass>(model.MasterClasses);
            Speeches = new ObservableCollection<Speech>(model.Speeches);
            Publications = new ObservableCollection<Publication>(model.Publications);
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

        // 1. AcademicYearResult (Итоговые результаты успеваемости)
        public ObservableCollection<AcademicYearResult> AcademicResults { get; }
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteAcademicResultCommand))]
        private AcademicYearResult? selectedAcademicResult;

        [RelayCommand]
        private void AddAcademicResult()
        {
            // Примерный формат для нового учебного года
            string currentYear = DateTime.Now.Year.ToString();
            string nextYear = (DateTime.Now.Year + 1).ToString();
            var newItem = new AcademicYearResult("Группа", $"{currentYear}-{nextYear}", "Новый предмет", null, null, null, null, null, null, null);
            AcademicResults.Add(newItem);
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

        // 2. GiaResult (Результаты ГИА) - НОВОЕ
        public ObservableCollection<GiaResult> GiaResults { get; }
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteGiaResultCommand))]
        private GiaResult? selectedGiaResult;

        [RelayCommand]
        private void AddGiaResult()
        {
            var newItem = new GiaResult("Предмет ГИА", "Группа", null, null, null, null, null, null, null, null);
            GiaResults.Add(newItem);
        }
        [RelayCommand(CanExecute = nameof(CanDeleteGiaResult))]
        private void DeleteGiaResult()
        {
            if (SelectedGiaResult != null)
            {
                GiaResults.Remove(SelectedGiaResult);
                SelectedGiaResult = null;
            }
        }
        private bool CanDeleteGiaResult() => SelectedGiaResult != null;

        // 3. DemoExamResult (Результаты ДЭ) - НОВОЕ
        public ObservableCollection<DemoExamResult> DemoExamResults { get; }
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteDemoExamResultCommand))]
        private DemoExamResult? selectedDemoExamResult;

        [RelayCommand]
        private void AddDemoExamResult()
        {
            var newItem = new DemoExamResult("Компетенция ДЭ", "Группа", null, null, null, null, null, null, null);
            DemoExamResults.Add(newItem);
        }
        [RelayCommand(CanExecute = nameof(CanDeleteDemoExamResult))]
        private void DeleteDemoExamResult()
        {
            if (SelectedDemoExamResult != null)
            {
                DemoExamResults.Remove(SelectedDemoExamResult);
                SelectedDemoExamResult = null;
            }
        }
        private bool CanDeleteDemoExamResult() => SelectedDemoExamResult != null;

        // 4. IndependentAssessment (Независимая оценка)
        public ObservableCollection<IndependentAssessment> IndependentAssessments { get; }
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteIndependentAssessmentCommand))]
        private IndependentAssessment? selectedIndependentAssessment;

        [RelayCommand]
        private void AddIndependentAssessment()
        {
            var newItem = new IndependentAssessment("Вид оценки", DateTime.Now.ToString("dd.MM.yyyy"), "Класс/Предмет", null, null, null, null, null, null, null);
            IndependentAssessments.Add(newItem);
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

        // 5. SelfDeterminationActivity (Профориентация)
        public ObservableCollection<SelfDeterminationActivity> SelfDeterminations { get; }
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteSelfDeterminationCommand))]
        private SelfDeterminationActivity? selectedSelfDetermination;

        [RelayCommand]
        private void AddSelfDetermination()
        {
            var newItem = new SelfDeterminationActivity("Уровень", "Мероприятие", "Роль", null);
            SelfDeterminations.Add(newItem);
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

        // 6. StudentOlympiad (Олимпиады обучающихся)
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

        // 7. JuryActivity (Работа в жюри)
        public ObservableCollection<JuryActivity> JuryActivities { get; }
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteJuryActivityCommand))]
        private JuryActivity? selectedJuryActivity;

        [RelayCommand]
        private void AddJuryActivity()
        {
            var newItem = new JuryActivity("Уровень", "Событие", DateTime.Now.ToString("dd.MM.yyyy"), null);
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

        // 8. MasterClass (Мастер-классы)
        public ObservableCollection<MasterClass> MasterClasses { get; }
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteMasterClassCommand))]
        private MasterClass? selectedMasterClass;

        [RelayCommand]
        private void AddMasterClass()
        {
            var newItem = new MasterClass("Уровень", "Тема", DateTime.Now.ToString("dd.MM.yyyy"), null);
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

        // 9. Speech (Выступления)
        public ObservableCollection<Speech> Speeches { get; }
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteSpeechCommand))]
        private Speech? selectedSpeech;

        [RelayCommand]
        private void AddSpeech()
        {
            var newItem = new Speech("Уровень", "Тема", DateTime.Now.ToString("dd.MM.yyyy"), null);
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

        // 10. Publication (Публикации)
        public ObservableCollection<Publication> Publications { get; }
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeletePublicationCommand))]
        private Publication? selectedPublication;

        [RelayCommand]
        private void AddPublication()
        {
            var newItem = new Publication("Уровень", "Название", DateTime.Now.ToString("dd.MM.yyyy"), null);
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

        // 11. ExperimentalProject (Экспериментальные проекты)
        public ObservableCollection<ExperimentalProject> ExperimentalProjects { get; }
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteExperimentalProjectCommand))]
        private ExperimentalProject? selectedExperimentalProject;

        [RelayCommand]
        private void AddExperimentalProject()
        {
            var newItem = new ExperimentalProject("Название проекта", DateTime.Now.ToString("dd.MM.yyyy"), null);
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

        // 12. Mentorship (Наставничество)
        public ObservableCollection<Mentorship> Mentorships { get; }
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteMentorshipCommand))]
        private Mentorship? selectedMentorship;

        [RelayCommand]
        private void AddMentorship()
        {
            var newItem = new Mentorship("Наставляемый", "Приказ", DateTime.Now.ToString("dd.MM.yyyy"), null);
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

        // 13. ProgramMethodSupport (Программно-методическое сопровождение)
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

        // 14. ProfessionalCompetition (Профессиональные конкурсы)
        public ObservableCollection<ProfessionalCompetition> ProfessionalCompetitions { get; }
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteProfessionalCompetitionCommand))]
        private ProfessionalCompetition? selectedProfessionalCompetition;

        [RelayCommand]
        private void AddProfessionalCompetition()
        {
            var newItem = new ProfessionalCompetition("Уровень", "Конкурс", "Достижение", DateTime.Now.ToString("dd.MM.yyyy"), null);
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

        // Логика для Trainings (Курсы повышения квалификации) УДАЛЕНА
    }
}