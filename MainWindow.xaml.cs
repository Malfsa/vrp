using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = DataContext as MainViewModel;
            if (viewModel == null) return;

            foreach (var item in e.RemovedItems)
            {
                viewModel.SelectedCities.Remove(item as City);
            }

            foreach (var item in e.AddedItems)
            {
                viewModel.SelectedCities.Add(item as City);
            }
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            Regex regex = null;

            if (textBox.Name == "NumberOfPercent")
            {
                // Разрешаем вводить числа с точкой
                regex = new Regex("[^0-9.]+");
            }
            else
            {
                // Разрешаем вводить только целые числа
                regex = new Regex("[^0-9]+");
            }

            e.Handled = regex.IsMatch(e.Text);
        }
        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }
    }
}