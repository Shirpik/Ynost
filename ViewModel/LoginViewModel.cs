using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Security; // Для SecureString

namespace Ynost.ViewModels
{
    public enum LoginResultRole
    {
        None,
        Editor,
        Viewer
    }

    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? _username;

        // Для пароля лучше использовать SecureString в реальных приложениях,
        // но для простоты примера с MVVM и привязками оставим пока string.
        // В WPF PasswordBox работает с SecureString.
        [ObservableProperty]
        private string? _password; // В реальном PasswordBox лучше привязать к SecureString

        [ObservableProperty]
        private string? _errorMessage;

        public LoginResultRole AuthenticatedUserRole { get; private set; } = LoginResultRole.None;
        public bool LoginSuccessful { get; private set; } = false;

        public IRelayCommand LoginCommand { get; }
        public Action? CloseAction { get; set; } // Для закрытия окна из ViewModel

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(AttemptLogin);
        }

        private void AttemptLogin()
        {
            ErrorMessage = null;
            LoginSuccessful = false;
            AuthenticatedUserRole = LoginResultRole.None;

            // Здесь должна быть реальная логика проверки логина и пароля.
            // Пока что захардкодим для примера.
            // Пароли в реальном приложении должны быть хешированы!
            if (Username == "admin" && Password == "admin")
            {
                AuthenticatedUserRole = LoginResultRole.Editor;
                LoginSuccessful = true;
            }
            else if (Username == "view" && Password == "view")
            {
                AuthenticatedUserRole = LoginResultRole.Viewer;
                LoginSuccessful = true;
            }
            else
            {
                ErrorMessage = "Неверный логин или пароль.";
                return;
            }

            CloseAction?.Invoke(); // Закрыть окно после успешного или неуспешного (с сообщением) логина
        }
    }
}