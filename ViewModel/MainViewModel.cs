using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using Ynost.Models;
using Ynost.Services;

namespace Ynost.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly DatabaseService _db;
        private bool _isInitialized = false;

        // Все свойства теперь объявлены явно без атрибутов
        private bool _isLoading;
        private string _loadingStatus = "Инициализация...";
        private bool _hasError;
        private string _errorMessage = string.Empty;

        public ObservableCollection<Teacher> Teachers { get; } = new();

        public MainViewModel(DatabaseService db)
        {
            _db = db;
            InitializeCommand = new AsyncRelayCommand(InitializeAsync);
        }

        public IAsyncRelayCommand InitializeCommand { get; }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        public string LoadingStatus
        {
            get => _loadingStatus;
            set => SetProperty(ref _loadingStatus, value);
        }

        public bool HasError
        {
            get => _hasError;
            set => SetProperty(ref _hasError, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        private async Task InitializeAsync()
        {
            if (_isInitialized || IsLoading) return;

            IsLoading = true;
            HasError = false;
            ErrorMessage = string.Empty;

            try
            {
                LoadingStatus = "Загрузка данных...";
                await LoadDataAsync(useCache: true);
                _isInitialized = true;
            }
            catch (Exception ex)
            {
                HasError = true;
                ErrorMessage = $"Ошибка инициализации: {ex.Message}";
                Debug.WriteLine($"Ошибка инициализации: {ex}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task LoadDataAsync(bool useCache = true)
        {
            try
            {
                var list = await _db.GetAllTeachersAsync(useCache);

                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    Teachers.Clear();
                    foreach (var t in list)
                        Teachers.Add(t);
                });

                if (useCache && !_isInitialized)
                {
                    _ = Task.Run(async () =>
                    {
                        await Task.Delay(2000);
                        await LoadDataAsync(useCache: false);
                    });
                }
            }
            catch (Exception ex)
            {
                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    ErrorMessage = $"Ошибка загрузки: {ex.Message}";
                    HasError = true;
                });

                Debug.WriteLine($"Ошибка загрузки данных: {ex}");
                throw;
            }
        }
    }
}