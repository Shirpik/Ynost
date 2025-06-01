using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Ynost.Services;
using Ynost.ViewModels;

namespace Ynost
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _vm;
        private readonly string _logPath;
        // private bool _isDataLoaded = false; // Состояние инициализации теперь в MainViewModel

        public MainWindow()
        {
            InitializeComponent();

            var connectionString = "Host=91.192.168.52;Port=5432;Database=ynost_db;Username=teacher_app;Password=T_pass;Ssl Mode=Disable";
            var dbService = new DatabaseService(connectionString);
            _vm = new MainViewModel(dbService);

            DataContext = _vm;
            Loaded += MainWindow_Loaded;

            // Инициализация _logPath должна быть здесь
            _logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ynost.log");
        }

        private async void MainWindow_Loaded(object? sender, RoutedEventArgs e)
        {
            // Флаг _isDataLoaded больше не нужен здесь, MainViewModel сам отслеживает инициализацию

            Log("=== Начало загрузки преподавателей (из MainWindow_Loaded) ===");
            var sw = Stopwatch.StartNew();

            try
            {
                // MainViewModel теперь сам управляет первой загрузкой (из кеша, если есть)
                // и последующим фоновым обновлением.
                await _vm.LoadDataAsync(useCache: true);

                sw.Stop();
                // Основное логирование и отображение статуса теперь внутри MainViewModel.
                // Можно добавить лог здесь, если нужно отследить именно завершение вызова из MainWindow_Loaded.
                Log($"Первичный вызов LoadDataAsync из MainWindow_Loaded завершен за {sw.ElapsedMilliseconds} ms. Статус ViewModel: {_vm.LoadingStatus}");
            }
            catch (Exception ex)
            {
                // Этот catch теперь менее вероятен, если MainViewModel обрабатывает ошибки и не пробрасывает их.
                // Но на всякий случай оставим для критических непредвиденных ошибок на этом уровне.
                sw.Stop();
                Log($"КРИТИЧЕСКАЯ ОШИБКА при вызове LoadDataAsync из MainWindow_Loaded: {ex}");
                MessageBox.Show(
                    $"Критическая ошибка при инициализации загрузки:\n{ex.Message}",
                    "Ynost — Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                Log("=== Окончание обработки MainWindow_Loaded ===\n");
            }
        }

        // Этот метод должен быть здесь, если он используется в XAML
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Убедись, что _vm.Teachers это ObservableCollection<TeacherViewModel>
            // Если это все еще ObservableCollection<Teacher>, то фильтрация немного изменится
            // Но исходя из предыдущих правок, это должна быть коллекция TeacherViewModel
            var view = CollectionViewSource.GetDefaultView(_vm.Teachers);
            if (view == null) return; // Добавим проверку на null

            string q = SearchBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(q))
            {
                view.Filter = null;
            }
            else
            {
                view.Filter = o =>
                    o is TeacherViewModel t && // Фильтруем по TeacherViewModel
                    t.FullName.Contains(q, StringComparison.OrdinalIgnoreCase);
            }

            Log($"Фильтр применён: \"{q}\" (осталось {view.Cast<object>().Count()} записей)");
        }

        // Этот метод должен быть здесь
        private void Log(string message)
        {
            try
            {
                // Используем _logPath, который инициализируется в конструкторе
                File.AppendAllText(_logPath,
                    $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} {message}{Environment.NewLine}");
            }
            catch { /* Подавление ошибок логирования, чтобы не нарушать работу приложения */ }
        }

        // Этот метод должен быть здесь, если он используется в XAML
        private void DataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!e.Handled && sender is UIElement)
            {
                if (DetailsScrollViewer != null) // DetailsScrollViewer должен иметь x:Name в XAML
                {
                    DetailsScrollViewer.ScrollToVerticalOffset(DetailsScrollViewer.VerticalOffset - e.Delta);
                    e.Handled = true;
                }
            }
        }

        // Этот метод может быть пустым или удален, если SelectedItem привязан в XAML
        // и дополнительная логика в CodeBehind не нужна.
        private void TeachersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Если SelectedItem в TeachersGrid привязан к _vm.SelectedTeacher (Mode=TwoWay),
            // то этот обработчик может быть не нужен для основной логики MVVM.
            // Оставляем его, так как он был в твоем коде.
        }
    }
}