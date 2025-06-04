using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using Ynost.Models;
using Ynost.Services;
using Ynost.ViewModels;
using Ynost.View;

namespace Ynost.ViewModels
{
    public partial class MainViewModel : ObservableObject // Убрали IDisposable, т.к. NetworkService удален
    {
        private readonly DatabaseService _db;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanEditData))]
        [NotifyCanExecuteChangedFor(nameof(RetryConnectionCommand))]
        private bool _isDatabaseConnected; // Теперь это основной флаг доступности БД

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ShowLoginPrompt))]
        [NotifyPropertyChangedFor(nameof(ShowDataGrid))]
        private bool _isLoading;
        [ObservableProperty]
        private string _loadingStatus = "Инициализация...";

        [ObservableProperty]
        private bool _isUsingCache; // Если данные были загружены из кеша из-за ошибки БД

        [ObservableProperty]
        private string _connectionStatusText = "Определение статуса...";

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveChangesCommand))]
        private bool _canEditData;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(LoginButtonText))]
        [NotifyPropertyChangedFor(nameof(UserStatusText))]
        [NotifyPropertyChangedFor(nameof(ShowLoginPrompt))]
        [NotifyPropertyChangedFor(nameof(ShowDataGrid))]
        [NotifyCanExecuteChangedFor(nameof(ToggleLoginCommand))]
        [NotifyCanExecuteChangedFor(nameof(RetryConnectionCommand))]
        private bool _isLoggedIn;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanEditData))]
        [NotifyPropertyChangedFor(nameof(UserStatusText))]
        private LoginResultRole _currentUserRole = LoginResultRole.None;

        public string LoginButtonText => IsLoggedIn ? "Выйти" : "Войти";
        public string UserStatusText => IsLoggedIn ? $" (Роль: {CurrentUserRole})" : string.Empty;

        public bool ShowLoginPrompt => !IsLoggedIn && !IsLoading;
        public bool ShowDataGrid => IsLoggedIn && !IsLoading;

        public ObservableCollection<TeacherViewModel> Teachers { get; } = new();
        [ObservableProperty]
        private TeacherViewModel? selectedTeacher;

        public IAsyncRelayCommand RetryConnectionCommand { get; }
        public IRelayCommand SaveChangesCommand { get; }
        public IRelayCommand ToggleLoginCommand { get; }

        public MainViewModel(DatabaseService db)
        {
            _db = db;

            RetryConnectionCommand = new AsyncRelayCommand(LoadDataAsync, CanRetryConnection);
            SaveChangesCommand = new RelayCommand(ExecuteSaveChanges, CanExecuteSaveChanges);
            ToggleLoginCommand = new RelayCommand(ExecuteToggleLogin);

            ConnectionStatusText = "Войдите в систему для начала работы.";
            UpdateCanEditDataProperty();
        }

        private void ExecuteToggleLogin()
        {
            if (IsLoggedIn)
            {
                var result = MessageBox.Show("Вы точно хотите выйти из аккаунта?", "Подтверждение выхода",
                                             MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    IsLoggedIn = false;
                    CurrentUserRole = LoginResultRole.None;
                    IsDatabaseConnected = false; // Сбрасываем статус соединения при выходе
                    Teachers.Clear();
                    ConnectionStatusText = "Вы вышли из системы. Войдите для доступа к данным.";
                    LoadingStatus = ConnectionStatusText;
                    UpdateCanEditDataProperty();
                    ((AsyncRelayCommand)RetryConnectionCommand).NotifyCanExecuteChanged();
                }
            }
            else
            {
                var loginWindow = new LoginWindow { Owner = Application.Current.MainWindow };
                bool? dialogResult = loginWindow.ShowDialog();
                if (dialogResult == true)
                {
                    var loginVm = loginWindow.DataContext as LoginViewModel;
                    if (loginVm != null && loginVm.LoginSuccessful)
                    {
                        IsLoggedIn = true;
                        CurrentUserRole = loginVm.AuthenticatedUserRole;
                        _ = LoadDataAsync();
                    }
                }
            }
            OnPropertyChanged(nameof(ShowLoginPrompt));
            OnPropertyChanged(nameof(ShowDataGrid));
        }

        private void UpdateCanEditDataProperty()
        {
            // Редактировать можно если: соединение с БД установлено, пользователь вошел И у него есть права редактора
            CanEditData = IsDatabaseConnected && IsLoggedIn && CurrentUserRole == LoginResultRole.Editor;
        }

        private bool CanRetryConnection()
        {
            // Повторить можно, если вошли в систему, но нет соединения с БД И не идет загрузка
            return IsLoggedIn && !IsDatabaseConnected && !IsLoading && !((AsyncRelayCommand)RetryConnectionCommand).IsRunning;
        }

        private void ExecuteSaveChanges()
        {
            // Проверка IsDatabaseConnected теперь основная
            if (!IsDatabaseConnected)
            {
                Application.Current.Dispatcher.Invoke(() => MessageBox.Show("Нет соединения с базой данных для сохранения.", "Ошибка сохранения", MessageBoxButton.OK, MessageBoxImage.Warning));
                return;
            }
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show("Функция сохранения будет реализована позже.", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }

        private bool CanExecuteSaveChanges()
        {
            return CanEditData;
        }

        public async Task LoadDataAsync()
        {
            if (!IsLoggedIn)
            {
                Teachers.Clear();
                ConnectionStatusText = "Войдите в систему для загрузки данных.";
                LoadingStatus = ConnectionStatusText;
                IsLoading = false;
                IsDatabaseConnected = false;
                UpdateCanEditDataProperty();
                ((AsyncRelayCommand)RetryConnectionCommand).NotifyCanExecuteChanged();
                OnPropertyChanged(nameof(ShowLoginPrompt));
                OnPropertyChanged(nameof(ShowDataGrid));
                return;
            }

            if (((AsyncRelayCommand)RetryConnectionCommand).IsRunning) return;

            IsLoading = true;
            LoadingStatus = "Загрузка данных...";
            IsUsingCache = false;
            // IsDatabaseConnected будет установлен ниже

            List<Teacher>? teacherModels = await _db.GetTeachersFromDbAsync();

            if (teacherModels != null)
            {
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    UpdateTeachersCollection(teacherModels);
                    IsDatabaseConnected = true;
                    IsUsingCache = false;
                    ConnectionStatusText = $"Данные успешно загружены из БД. Записей: {Teachers.Count}.";
                });
                await _db.SaveTeachersToCacheAsync(teacherModels);
            }
            else
            {
                IsDatabaseConnected = false;
                List<Teacher>? cachedModels = await _db.LoadTeachersFromCacheAsync();
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    if (cachedModels != null && cachedModels.Count > 0)
                    {
                        UpdateTeachersCollection(cachedModels);
                        IsUsingCache = true;
                        ConnectionStatusText = "Ошибка соединения с БД. Отображаются данные из кеша.";
                    }
                    else
                    {
                        Teachers.Clear();
                        IsUsingCache = false;
                        ConnectionStatusText = "Ошибка соединения с БД. Кеш пуст или недоступен.";
                    }
                });
            }

            IsLoading = false;
            LoadingStatus = ConnectionStatusText; // Обновляем общий статус загрузки
            UpdateCanEditDataProperty();
            ((AsyncRelayCommand)RetryConnectionCommand).NotifyCanExecuteChanged();
            OnPropertyChanged(nameof(ShowLoginPrompt));
            OnPropertyChanged(nameof(ShowDataGrid));
        }

        private void UpdateTeachersCollection(List<Teacher> newTeacherModels)
        {
            int? previouslySelectedTeacherId = SelectedTeacher?.GetModelId();
            Teachers.Clear();
            if (newTeacherModels != null)
            {
                foreach (var model in newTeacherModels)
                {
                    Teachers.Add(new TeacherViewModel(model));
                }
            }

            if (previouslySelectedTeacherId.HasValue)
            {
                SelectedTeacher = Teachers.FirstOrDefault(tvm => tvm.GetModelId() == previouslySelectedTeacherId.Value);
            }
            if (SelectedTeacher == null && Teachers.Count > 0)
            {
                SelectedTeacher = Teachers[0];
            }
        }
    }
}