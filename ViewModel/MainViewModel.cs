using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Ynost.Services;

namespace Ynost.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly DatabaseService _db;

        // Теперь — коллекция VM, а не просто моделей
        public ObservableCollection<TeacherViewModel> Teachers { get; } = new();

        // Свойство для выбранного преподавателя
        [ObservableProperty]
        private TeacherViewModel? selectedTeacher;

        public MainViewModel(DatabaseService db)
        {
            _db = db;
        }

        public async Task LoadDataAsync()
        {
            var list = await _db.GetAllTeachersAsync();
            Teachers.Clear();
            foreach (var t in list)
            {
                Teachers.Add(new TeacherViewModel(t));
            }

            // Выбираем первого по умолчанию
            if (Teachers.Count > 0)
                SelectedTeacher = Teachers[0];
        }
    }
}
