using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
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
            Log("=== Начало загрузки преподавателей (из MainWindow_Loaded) ===");
            var sw = Stopwatch.StartNew();
            try
            {
                await _vm.LoadDataAsync(); // Вызов без параметров

                sw.Stop();
                Log($"Операция загрузки в MainWindow_Loaded завершена за {sw.ElapsedMilliseconds} ms. Статус ViewModel: {_vm.ConnectionStatusText}");
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

        private void EditingTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var textBox = sender as TextBox;
                if (textBox == null) return;

                if ((Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.Shift)
                {
                    e.Handled = true;

                    DependencyObject parent = VisualTreeHelper.GetParent(textBox);
                    DataGrid grid = null;
                    while (parent != null)
                    {
                        grid = parent as DataGrid;
                        if (grid != null) break;
                        parent = VisualTreeHelper.GetParent(parent);
                    }

                    if (grid != null)
                    {
                        grid.CommitEdit(DataGridEditingUnit.Row, true);
                    }
                }
            }
        }
        // Вызывается, когда ячейка входит в режим редактирования
        private void DataGrid_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            if (e.EditingElement is TextBox textBox)
            {
                textBox.PreviewKeyDown -= EditingTextBox_PreviewKeyDown;
                textBox.PreviewKeyDown += EditingTextBox_PreviewKeyDown;
            }
        }

        // Вызывается, когда редактирование ячейки завершается или отменяется
        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditingElement is TextBox textBox)
            {
                textBox.PreviewKeyDown -= EditingTextBox_PreviewKeyDown;
            }
        }
    }
}