using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input; // Для IRelayCommand
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using Ynost.Models;
using Ynost.Services;
using Ynost.ViewModels;

namespace Ynost.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly DatabaseService _db;

        [ObservableProperty]
        private bool _isLoading;
        [ObservableProperty]
        private string _loadingStatus = "Инициализация...";

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RetryConnectionCommand))] // Обновляем состояние кнопки
        private bool _isDatabaseConnected;

        [ObservableProperty]
        private bool _isUsingCache;

        [ObservableProperty]
        private string _connectionStatusText = "Определение статуса...";

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveChangesCommand))] // Обновляем состояние кнопки
        private bool _canEditData;

        public ObservableCollection<TeacherViewModel> Teachers { get; } = new();
        [ObservableProperty]
        private TeacherViewModel? selectedTeacher;

        public IRelayCommand RetryConnectionCommand { get; }
        public IRelayCommand SaveChangesCommand { get; }


        public MainViewModel(DatabaseService db)
        {
            _db = db;
            RetryConnectionCommand = new AsyncRelayCommand(LoadDataAsync, CanRetryConnection);
            SaveChangesCommand = new RelayCommand(ExecuteSaveChanges, CanExecuteSaveChanges);
        }

        private bool CanRetryConnection()
        {
            return !IsDatabaseConnected && !IsLoading; // Можно повторить, если нет соединения и не идет загрузка
        }

        private void ExecuteSaveChanges()
        {
            // Пока пустышка
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show("Функция сохранения будет реализована позже.", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
            });
            Debug.WriteLine("[MainViewModel] SaveChangesCommand executed (placeholder).");
            // В будущем здесь будет логика отправки изменений в DatabaseService.
            // После попытки сохранения, нужно будет обновить IsDatabaseConnected, CanEditData, ConnectionStatusText
            // в зависимости от результата операции сохранения.
        }

        private bool CanExecuteSaveChanges()
        {
            return CanEditData; // Можно сохранить, если разрешено редактирование
        }


        public async Task LoadDataAsync()
        {
            if (IsLoading && (RetryConnectionCommand as AsyncRelayCommand)?.IsRunning == true) return; // Предотвращаем многократный запуск, если уже идет попытка

            IsLoading = true;
            LoadingStatus = "Соединение с базой данных...";
            ConnectionStatusText = "Попытка соединения с БД...";
            IsDatabaseConnected = false;
            IsUsingCache = false;
            CanEditData = false;
            // Обновляем состояние команд, т.к. IsLoading и IsDatabaseConnected изменились
            ((AsyncRelayCommand)RetryConnectionCommand).NotifyCanExecuteChanged();
            ((RelayCommand)SaveChangesCommand).NotifyCanExecuteChanged();


            List<Teacher>? teacherModels = await _db.GetTeachersFromDbAsync();

            if (teacherModels != null)
            {
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    UpdateTeachersCollection(teacherModels);
                    IsDatabaseConnected = true;
                    IsUsingCache = false;
                    CanEditData = true;
                    ConnectionStatusText = $"Данные успешно загружены из БД. Записей: {Teachers.Count}.";
                    LoadingStatus = ConnectionStatusText;
                });
                await _db.SaveTeachersToCacheAsync(teacherModels);
            }
            else
            {
                LoadingStatus = "Ошибка соединения с БД, загрузка из кеша...";
                ConnectionStatusText = "Ошибка соединения с БД. Попытка загрузки из кеша...";

                List<Teacher>? cachedModels = await _db.LoadTeachersFromCacheAsync();
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    if (cachedModels != null && cachedModels.Count > 0)
                    {
                        UpdateTeachersCollection(cachedModels);
                        IsUsingCache = true;
                        ConnectionStatusText = $"Отображаются данные из кеша (ошибка БД). Записей: {Teachers.Count}. Редактирование запрещено.";
                    }
                    else
                    {
                        Teachers.Clear();
                        ConnectionStatusText = "Ошибка соединения с БД. Кеш пуст или недоступен. Данные не загружены.";
                    }
                    IsDatabaseConnected = false;
                    CanEditData = false;
                    LoadingStatus = ConnectionStatusText;
                });
            }

            IsLoading = false;
            // Обновляем состояние команд после завершения загрузки
            ((AsyncRelayCommand)RetryConnectionCommand).NotifyCanExecuteChanged();
            ((RelayCommand)SaveChangesCommand).NotifyCanExecuteChanged();
        }

        private void UpdateTeachersCollection(List<Teacher> newTeacherModels)
        {
            int? previouslySelectedTeacherId = SelectedTeacher?.GetModelId();
            Teachers.Clear();
            foreach (var model in newTeacherModels)
            {
                Teachers.Add(new TeacherViewModel(model));
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