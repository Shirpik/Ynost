using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media; 
using Ynost.Services;      
using Ynost.ViewModels;  

namespace Ynost.View
{
    public partial class MainWindow
    {
        private readonly MainViewModel _vm;
        private readonly string _logPath;

        public MainWindow()
        {
            InitializeComponent();

            // Строка подключения - вынесена для наглядности
            const string connectionString = "Host=91.192.168.52;Port=5432;Database=ynost_db;Username=teacher_app;Password=T_pass;Ssl Mode=Disable";
            var dbService = new DatabaseService(connectionString);
            _vm = new MainViewModel(dbService);

            DataContext = _vm;
            Loaded += MainWindow_Loaded;

            _logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ynost.log");
        }

        private async void MainWindow_Loaded(object? sender, RoutedEventArgs e)
        {
            Log("=== Начало загрузки данных при старте окна (MainWindow_Loaded) ===");
            var sw = Stopwatch.StartNew();
            try
            {
                // MainViewModel сам решит, нужно ли грузить данные (в зависимости от IsLoggedIn)
                await _vm.LoadDataAsync();

                sw.Stop();
                Log($"Операция MainWindow_Loaded завершена за {sw.ElapsedMilliseconds} ms. Статус ViewModel: {_vm.ConnectionStatusText}");
            }
            catch (Exception ex)
            {
                sw.Stop();
                Log($"КРИТИЧЕСКАЯ ОШИБКА при вызове LoadDataAsync из MainWindow_Loaded: {ex}"); // ToString() для полной информации об ошибке
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
            var view = CollectionViewSource.GetDefaultView(_vm.Teachers);
            if (view == null) return;

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
            catch { /* Подавление ошибок логирования */ }
        }

        private void DataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!e.Handled && sender is UIElement)
            {
                if (DetailsScrollViewer != null)
                {
                    DetailsScrollViewer.ScrollToVerticalOffset(DetailsScrollViewer.VerticalOffset - e.Delta);
                    e.Handled = true;
                }
            }
        }

        // Обработчик для Enter/Shift+Enter в редактируемых ячейках
        private void EditingTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var textBox = sender as TextBox;
                if (textBox == null) return;

                if ((Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.Shift)
                {
                    e.Handled = true;

                    DependencyObject? parent = VisualTreeHelper.GetParent(textBox);
                    DataGrid? grid = null;
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
                // Если нажат Shift+Enter, TextBox с AcceptsReturn=True обработает это как новую строку
            }
        }

        // Подписка на PreviewKeyDown для TextBox при входе в режим редактирования
        private void DataGrid_PreparingCellForEdit(object sender, DataGridPreparingCellForEditEventArgs e)
        {
            if (e.EditingElement is TextBox textBox)
            {
                textBox.PreviewKeyDown -= EditingTextBox_PreviewKeyDown;
                textBox.PreviewKeyDown += EditingTextBox_PreviewKeyDown;
            }
        }

        // Отписка от PreviewKeyDown при завершении редактирования
        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditingElement is TextBox textBox)
            {
                textBox.PreviewKeyDown -= EditingTextBox_PreviewKeyDown;
            }
        }
    }
}