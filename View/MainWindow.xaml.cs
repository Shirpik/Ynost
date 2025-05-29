using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Ynost.Services;
using Ynost.ViewModels;

namespace Ynost
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _vm;
        private readonly string _logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ynost.log");

        public MainWindow()
        {
            InitializeComponent();

            var connectionString =
                "Host=91.192.168.52;Port=5432;Database=ynost_db;Username=teacher_app;Password=T_pass;Ssl Mode=Disable";
            var dbService = new DatabaseService(connectionString);
            _vm = new MainViewModel(dbService);

            DataContext = _vm;
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object? sender, RoutedEventArgs e)
        {
            Log("=== Начало загрузки преподавателей ===");
            var sw = Stopwatch.StartNew();

            try
            {
                await _vm.LoadDataAsync();
                sw.Stop();
                Log($"Успех: загружено {_vm.Teachers.Count} преподавателей за {sw.ElapsedMilliseconds} ms");
                MessageBox.Show(
                    $"Загружено преподавателей: {_vm.Teachers.Count}",
                    "Ynost",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                sw.Stop();
                Log($"ОШИБКА при загрузке: {ex}");
                MessageBox.Show(
                    $"Ошибка при загрузке преподавателей:\n{ex.Message}",
                    "Ynost — Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                Log("=== Конец операции ===\n");
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var view = CollectionViewSource.GetDefaultView(_vm.Teachers);
            string q = SearchBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(q))
            {
                view.Filter = null;
            }
            else
            {
                view.Filter = o =>
                    o is TeacherViewModel t &&
                    t.FullName.Contains(q, StringComparison.OrdinalIgnoreCase);
            }

            Log($"Фильтр применён: \"{q}\" (осталось {view.Cast<object>().Count()} записей)");
        }

        private void Log(string message)
        {
            try
            {
                File.AppendAllText(_logPath,
                    $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} {message}{Environment.NewLine}");
            }
            catch { }
        }
    }
}
