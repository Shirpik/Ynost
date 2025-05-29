using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Ynost.Models;
using Ynost.Services;

namespace Ynost.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly DatabaseService _db;

        public ObservableCollection<Teacher> Teachers { get; } = new();

        public MainViewModel(DatabaseService db)
        {
            _db = db;
        }

        public async Task LoadDataAsync()
        {
            var list = await _db.GetAllTeachersAsync();
            Teachers.Clear();
            foreach (var t in list)
                Teachers.Add(t);
        }
    }
}
