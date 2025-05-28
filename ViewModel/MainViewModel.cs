using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ynost.Models;
namespace Ynost.ViewModels;
public partial class MainViewModel : ObservableObject
{
    public MainViewModel()
    {
        Teachers = new ObservableCollection<TeacherViewModel>
        {
            DemoTeacherFactory.BuildIvanov(),
            DemoTeacherFactory.BuildPetrov()
        };
        SelectedTeacher = Teachers[0];
    }
    public ObservableCollection<TeacherViewModel> Teachers { get; }
    [ObservableProperty] private TeacherViewModel? selectedTeacher;
    public bool IsAdmin { get; set; }
    [RelayCommand] private void Refresh() { }
    [RelayCommand] private void Save() { if (IsAdmin) { } }
}