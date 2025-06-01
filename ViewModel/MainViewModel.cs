// MainViewModel.cs
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using Ynost.Models; // Убедись, что этот using есть
using Ynost.Services;
using Ynost.ViewModels; // Добавь этот using для TeacherViewModel

namespace Ynost.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly DatabaseService _db;
        private bool _isInitialized = false;

        // Свойства для состояния загрузки (оставляем твои)
        [ObservableProperty]
        private bool _isLoading;
        [ObservableProperty]
        private string _loadingStatus = "Инициализация...";
        [ObservableProperty]
        private bool _hasError;
        [ObservableProperty]
        private string _errorMessage = string.Empty;

        // Коллекция теперь будет содержать TeacherViewModel
        public ObservableCollection<TeacherViewModel> Teachers { get; } = new();

        // Свойство для выбранного преподавателя (TeacherViewModel)
        [ObservableProperty]
        private TeacherViewModel? selectedTeacher;


        public MainViewModel(DatabaseService db)
        {
            _db = db;
            // InitializeCommand больше не нужен, так как загрузка идет из MainWindow.xaml.cs
            // Если ты его используешь где-то еще, оставь. Для простоты пока уберу.
        }

        // Убираем InitializeCommand и InitializeAsync, если MainWindow.xaml.cs управляет загрузкой
        // Если ты его используешь, оставь, но убедись, что SelectedTeacher устанавливается.

        public async Task LoadDataAsync(bool useCache = true)
        {
            IsLoading = true; // Установим IsLoading в начале
            HasError = false;
            ErrorMessage = string.Empty;
            if (useCache && !_isInitialized)
            {
                LoadingStatus = "Загрузка из кеша...";
            }
            else if (!useCache)
            {
                LoadingStatus = "Обновление данных с сервера...";
            }


            try
            {
                var teacherModels = await _db.GetAllTeachersAsync(useCache); // Получаем список Teacher

                // Обновляем коллекцию Teachers в UI потоке
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    // Сохраняем текущий выбранный Id, если он есть
                    int? previouslySelectedTeacherId = SelectedTeacher?.GetModelId();

                    Teachers.Clear();
                    foreach (var model in teacherModels)
                    {
                        Teachers.Add(new TeacherViewModel(model)); // Оборачиваем в TeacherViewModel
                    }

                    // Попытка восстановить выбор
                    if (previouslySelectedTeacherId.HasValue)
                    {
                        SelectedTeacher = Teachers.FirstOrDefault(tvm => tvm.GetModelId() == previouslySelectedTeacherId.Value);
                    }

                    // Если после обновления выбранный элемент стал null (например, его удалили)
                    // или если это первая загрузка, выбираем первого в списке.
                    if (SelectedTeacher == null && Teachers.Count > 0)
                    {
                        SelectedTeacher = Teachers[0];
                    }
                });

                if (!_isInitialized) _isInitialized = true;

                // Фоновое обновление, если это была загрузка из кеша (твоя логика)
                // Убедимся, что оно не вызывается рекурсивно, если isInitialized уже true
                // и это был не первый вызов LoadDataAsync
                if (useCache && _isInitialized) // Если грузили из кеша и уже инициализированы
                {
                    // Запускаем полное обновление, если это не оно само себя вызвало из фонового потока
                    bool isThisTheBackgroundUpdateCall = !Application.Current.Dispatcher.CheckAccess();
                    if (!isThisTheBackgroundUpdateCall) // Т.е. если это основной вызов LoadDataAsync(useCache:true)
                    {
                        _ = Task.Run(async () =>
                        {
                            await Task.Delay(2000); // Небольшая задержка перед фоновым обновлением
                            await LoadDataAsync(useCache: false); // Полное обновление
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    ErrorMessage = $"Ошибка загрузки: {ex.Message}";
                    HasError = true;
                    LoadingStatus = "Ошибка загрузки";
                });
                Debug.WriteLine($"Ошибка загрузки данных: {ex}");
                // throw; // Не бросаем исключение дальше, чтобы UI не падал, ошибки отображаются
            }
            finally
            {
                IsLoading = false; // Сбрасываем IsLoading в конце
                if (!HasError) LoadingStatus = $"Загружено: {Teachers.Count} преподавателей";
            }
        }
    }
}