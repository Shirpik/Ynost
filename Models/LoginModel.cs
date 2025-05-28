using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data;

public partial class LoginViewModel : ObservableObject
{
    [ObservableProperty] private string _login = "";
    [ObservableProperty] private string _password = "";
}