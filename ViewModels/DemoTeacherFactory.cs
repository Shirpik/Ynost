using Ynost.Models;

namespace Ynost.ViewModels
{
    internal static class DemoTeacherFactory
    {
        public static TeacherViewModel BuildIvanov()
        {
            var t = new Teacher { Id = 1, FullName = "Иванов Иван Иванович", IsLecturer = true };

            t.AcademicResults.AddRange([
                new AcademicYearResult("МИ-8", "2023-2024","Математика","4.2","4.4","+0.2","73.5","95.0","0.95",null),
                new AcademicYearResult("ПИ-3", "2022-2023","Математика","4.0","4.2","+0.2","71.0","94.0","0.94",null),
                new AcademicYearResult("СУ-100", "2021-2022","Математика","3.8","4.0","+0.2","68.0","92.0","0.92",null)
            ]);

            t.GiaResults.Add(new GiaResult("Математика", "11Б", "22", "10.0", "25.0", "55.0", "5.0", "5.0", "75.5", null));
            t.GiaResults.Add(new GiaResult("Математика", "9А", "28", "15.0", "30.0", "45.0", "10.0", "0.0", "4.15", null));

            t.DemoExamResults.Add(new DemoExamResult("Сетевое и системное администрирование", "ИС-41", "20", "25.0", "50.0", "20.0", "5.0", "4.5", null));

            t.IndependentAssessments.Add(new IndependentAssessment(
                "ВПР", new DateTime(2024, 4, 10).ToString("dd.MM.yyyy"), "7А/Математика",
                "27", "27", "24",
                "88.8", "90.0", "0.9",
                null));

            t.SelfDeterminations.Add(new SelfDeterminationActivity("муниципальный", "День профессий", "координатор", null));
            t.StudentOlympiads.Add(new StudentOlympiad("региональный", "Кенгуру", "очно", "Петрова А.А.", "призёр", null));
            t.JuryActivities.Add(new JuryActivity("региональный", "Олимпиада школьников", new DateTime(2023, 12, 1).ToString("dd.MM.yyyy"), null));
            t.MasterClasses.Add(new MasterClass("школьный", "GeoGebra на уроке", new DateTime(2024, 2, 18).ToString("dd.MM.yyyy"), null));
            t.Speeches.Add(new Speech("вуз", "Методика решения задач", new DateTime(2023, 10, 5).ToString("dd.MM.yyyy"), null));
            t.Publications.Add(new Publication("региональный", "Игровые технологии", new DateTime(2022, 6, 1).ToString("dd.MM.yyyy"), null));
            t.ExperimentalProjects.Add(new ExperimentalProject("Гибридное обучение", new DateTime(2024, 1, 20).ToString("dd.MM.yyyy"), null));
            t.Mentorships.Add(new Mentorship("Сидоров С.С.", "№15-н", new DateTime(2023, 9, 10).ToString("dd.MM.yyyy"), null));
            t.ProgramSupports.Add(new ProgramMethodSupport("Алгебра 7–9", true, null));
            t.ProfessionalCompetitions.Add(new ProfessionalCompetition("федеральный", "Учитель года", "участник", new DateTime(2023, 5, 15).ToString("dd.MM.yyyy"), null));

            return new TeacherViewModel(t);
        }

        public static TeacherViewModel BuildPetrov()
        {
            var p = new Teacher { Id = 2, FullName = "Петров Пётр Петрович", IsLecturer = false };

            p.AcademicResults.Add(new AcademicYearResult("ДИ-20", "2023-2024", "Физика", "4.0", "4.5", "+0.5", "78.0", "97.0", "0.97", null));

            p.GiaResults.Add(new GiaResult("Физика", "11А", "18", "12.5", "50.0", "35.0", "2.5", "2.5", "70.0", null));
            p.GiaResults.Add(new GiaResult("Физика", "9Б", "25", "20.0", "40.0", "30.0", "10.0", "0.0", "4.1", null));

            p.DemoExamResults.Add(new DemoExamResult("Ремонт и обслуживание легковых автомобилей", "ТО-31", "15", "20.0", "60.0", "15.0", "5.0", "4.2", null));

            p.IndependentAssessments.Add(new IndependentAssessment(
                "НИКО", new DateTime(2024, 4, 15).ToString("dd.MM.yyyy"), "8Б/Физика",
                "26", "26", "22",
                "84.6", "85.0", "0.85",
                null));

            p.SelfDeterminations.Add(new SelfDeterminationActivity("региональный", "Форум профессий", "организатор", null));
            p.StudentOlympiads.Add(new StudentOlympiad("всероссийский", "Шаг в науку", "дист.", "Сидоров С.С.", "лауреат", null));
            p.JuryActivities.Add(new JuryActivity("муниципальный", "Конкурс роботов", new DateTime(2023, 11, 10).ToString("dd.MM.yyyy"), null));
            p.MasterClasses.Add(new MasterClass("муниципальный", "STEM-урок", new DateTime(2024, 2, 20).ToString("dd.MM.yyyy"), null));
            p.Speeches.Add(new Speech("региональный", "STEAM-круглый стол", new DateTime(2024, 3, 5).ToString("dd.MM.yyyy"), null));
            p.Publications.Add(new Publication("региональный", "Arduino на уроках", new DateTime(2022, 10, 1).ToString("dd.MM.yyyy"), null));
            p.ExperimentalProjects.Add(new ExperimentalProject("VR-лаборатория", new DateTime(2024, 1, 15).ToString("dd.MM.yyyy"), null));
            p.Mentorships.Add(new Mentorship("Кузнецов К.К.", "№12-к", new DateTime(2023, 9, 12).ToString("dd.MM.yyyy"), null));
            p.ProgramSupports.Add(new ProgramMethodSupport("Физика 10–11", true, null));
            p.ProfessionalCompetitions.Add(new ProfessionalCompetition("региональный", "Учитель года", "финалист", new DateTime(2022, 5, 20).ToString("dd.MM.yyyy"), null));

            return new TeacherViewModel(p);
        }
    }
}