using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Ynost.ViewModels;

namespace Ynost
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataContext is not MainViewModel vm) return;

            string query = SearchBox.Text.Trim().ToLower();

            // Если строка пустая — показываем всех
            var filtered = string.IsNullOrWhiteSpace(query)
                ? vm.Teachers.ToList()
                : vm.Teachers.Where(t => t.FullName.ToLower().Contains(query)).ToList();

            TeachersGrid.ItemsSource = filtered;
        }
    }
}
