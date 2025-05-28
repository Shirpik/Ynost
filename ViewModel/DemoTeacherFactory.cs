using Ynost.Models;
namespace Ynost.ViewModels;
internal static class DemoTeacherFactory
{
    public static TeacherViewModel BuildIvanov()
    {
        var t = new Teacher { Id = 1, FullName = "Иванов Иван Иванович" };
        t.Trainings.Add(new TrainingCourse("ИКТ в образовании", 72, 2024, "ИРО", null));
        return new TeacherViewModel(t);
    }
    public static TeacherViewModel BuildPetrov()
    {
        var p = new Teacher { Id = 2, FullName = "Петров Пётр Петрович" };
        p.Trainings.Add(new TrainingCourse("STEM-обучение", 48, 2023, "ФИРО", null));
        return new TeacherViewModel(p);
    }
}