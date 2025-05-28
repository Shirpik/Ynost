using Ynost.Models;
namespace Ynost.ViewModels;
internal static class DemoTeacherFactory
{
    public static TeacherViewModel BuildIvanov()
    {
        var t = new Teacher { Id = 1, FullName = "Иванов Иван Иванович" };

        t.AcademicResults.AddRange(new[] {
            new AcademicYearResult(2024,"Математика",4.2,4.4,+0.2,4.3,73.5,0.95,null),
            new AcademicYearResult(2023,"Математика",4.0,4.2,+0.2,4.1,71.0,0.94,null),
            new AcademicYearResult(2022,"Математика",3.8,4.0,+0.2,3.9,68.0,0.92,null)});

        t.EgeResults.Add(new EgeResult("Математика", "11Б", 22, 10, 55, 30, 5, null));
        t.OgeResults.Add(new OgeResult("Математика", "9А", 28, 4, 10, 12, 2, 4.15, null));
        t.IndependentAssessments.Add(new IndependentAssessment("ВПР", new DateTime(2024, 4, 10), "7А/Математика", 27, 27, 24, null));
        t.SelfDeterminations.Add(new SelfDeterminationActivity("муниципальный", "День профессий", "координатор", null));
        t.StudentOlympiads.Add(new StudentOlympiad("региональный", "Кенгуру", "очно", "Петрова А.А.", "призёр", null));
        t.JuryActivities.Add(new JuryActivity("региональный", "Олимпиада школьников", new DateTime(2023, 12, 1), null));
        t.MasterClasses.Add(new MasterClass("школьный", "GeoGebra на уроке", new DateTime(2024, 2, 18), null));
        t.Speeches.Add(new Speech("вуз", "Методика решения задач", new DateTime(2023, 10, 5), null));
        t.Publications.Add(new Publication("региональный", "Игровые технологии", new DateTime(2022, 6, 1), null));
        t.ExperimentalProjects.Add(new ExperimentalProject("Гибридное обучение", new DateTime(2024, 1, 20), null));
        t.Mentorships.Add(new Mentorship("Сидоров С.С.", "№15-н", new DateTime(2023, 9, 10), null));
        t.ProgramSupports.Add(new ProgramMethodSupport("Алгебра 7–9", true, null));
        t.ProfessionalCompetitions.Add(new ProfessionalCompetition("федеральный", "Учитель года", "участник", new DateTime(2023, 5, 15), null));
        t.Trainings.AddRange(new[] {
            new TrainingCourse("ИКТ в образовании",72,2024,"ИРО",null),
            new TrainingCourse("Практикум CLIL",36,2023,"МГПУ",null)});
        return new TeacherViewModel(t);
    }

    public static TeacherViewModel BuildPetrov()
    {
        var p = new Teacher { Id = 2, FullName = "Петров Пётр Петрович" };

        p.AcademicResults.Add(new AcademicYearResult(2024, "Физика", 4.0, 4.5, +0.5, 4.25, 78, 0.97, null));
        p.EgeResults.Add(new EgeResult("Физика", "11А", 18, 12.5, 50, 35, 2.5, null));
        p.OgeResults.Add(new OgeResult("Физика", "9Б", 25, 5, 8, 10, 2, 4.1, null));
        p.IndependentAssessments.Add(new IndependentAssessment("ВПР", new DateTime(2024, 4, 15), "7Б/Физика", 26, 26, 22, null));
        p.SelfDeterminations.Add(new SelfDeterminationActivity("региональный", "Форум профессий", "организатор", null));
        p.StudentOlympiads.Add(new StudentOlympiad("всероссийский", "Шаг в науку", "дист.", "Сидоров С.С.", "лауреат", null));
        p.JuryActivities.Add(new JuryActivity("муниципальный", "Конкурс роботов", new DateTime(2023, 11, 10), null));
        p.MasterClasses.Add(new MasterClass("муниципальный", "STEM-урок", new DateTime(2024, 2, 20), null));
        p.Speeches.Add(new Speech("региональный", "STEAM-круглый стол", new DateTime(2024, 3, 5), null));
        p.Publications.Add(new Publication("региональный", "Arduino на уроках", new DateTime(2022, 10, 1), null));
        p.ExperimentalProjects.Add(new ExperimentalProject("VR-лаборатория", new DateTime(2024, 1, 15), null));
        p.Mentorships.Add(new Mentorship("Кузнецов К.К.", "№12-к", new DateTime(2023, 9, 12), null));
        p.ProgramSupports.Add(new ProgramMethodSupport("Физика 10–11", true, null));
        p.ProfessionalCompetitions.Add(new ProfessionalCompetition("региональный", "Учитель года", "финалист", new DateTime(2022, 5, 20), null));
        p.Trainings.AddRange(new[] {
            new TrainingCourse("STEM-обучение",48,2023,"ФИРО",null),
            new TrainingCourse("VR-технологии",24,2024,"Физмат-центр",null)});
        return new TeacherViewModel(p);
    }
}