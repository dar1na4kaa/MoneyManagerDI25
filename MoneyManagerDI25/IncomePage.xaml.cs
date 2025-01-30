using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MoneyManagerX.Service;

namespace MoneyManagerX
{
    public partial class IncomePage : Page
    {
        private TransactionService _transactionService;
        private CategoryService _categoryService;
        private User _user;
        private DateTime _startDate;
        private DateTime _endDate;
        private int? _selectedAccountId;
        private List<Category> _selectedCategories;

        public IncomePage(User user)
        {
            InitializeComponent();
            _user = user;
            _transactionService = new TransactionService();
            _categoryService = new CategoryService();
            LoadAccounts();
            SetDefaultPeriod();
            _selectedCategories = new List<Category>(); 
        }

        private void LoadAccounts()
        {
            AccountComboBox.ItemsSource = _user.Accounts;
            AccountComboBox.DisplayMemberPath = "Name";
            AccountComboBox.SelectedValuePath = "Id";

            if (_user.Accounts.Any())
            {
                AccountComboBox.SelectedIndex = 0;
            }
        }

        private void AccountComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AccountComboBox.SelectedValue != null)
            {
                _selectedAccountId = (int)AccountComboBox.SelectedValue;
                LoadIncomeData();
            }
        }

        private void SetDefaultPeriod()
        {
            _endDate = DateTime.Today;
            _startDate = _endDate.AddDays(-30);
            LoadIncomeData();
        }

        private void PeriodComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PeriodComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string tag = selectedItem.Tag.ToString();

                if (tag == "custom")
                {
                    CustomDatePanel.Visibility = Visibility.Visible;
                }
                else
                {
                    int days = int.Parse(tag);
                    _endDate = DateTime.Today;
                    _startDate = _endDate.AddDays(-days);
                    CustomDatePanel.Visibility = Visibility.Collapsed;
                    LoadIncomeData();
                }
            }
        }

        private void ApplyCustomPeriod(object sender, RoutedEventArgs e)
        {
            if (StartDatePicker.SelectedDate.HasValue && EndDatePicker.SelectedDate.HasValue)
            {
                _startDate = StartDatePicker.SelectedDate.Value;
                _endDate = EndDatePicker.SelectedDate.Value;
                LoadIncomeData();
            }
            else
            {
                MessageBox.Show("Выберите начальную и конечную дату!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void LoadIncomeData()
        {
            if (_selectedAccountId.HasValue)
            {
                var selectedCategories = _selectedCategories; // Ваши выбранные категории
                var incomes = _transactionService.GetIncomeForUser(selectedCategories, _startDate, _endDate);

                pieChart.Series.Clear();

                foreach (var income in incomes)
                {
                    pieChart.Series.Add(new LiveCharts.Wpf.PieSeries
                    {
                        Values = new LiveCharts.ChartValues<decimal> { income.Amount },
                        Title = income.Category.Name,
                        DataLabels = true
                    });
                }
            }
        }



        private void OnAddIncomeClick(object sender, RoutedEventArgs e)
        {
            TransactionWindow transactionWindow = new TransactionWindow(_user);
            transactionWindow.Show();
        }

        private void OnAddCategoryClick(object sender, RoutedEventArgs e)
        {
            var categorySelectionWindow = new CategorySelectionWindow(_user);
            categorySelectionWindow.ShowDialog();

            // Получаем выбранные категории и обновляем данные
            _selectedCategories = categorySelectionWindow.GetSelectedCategories();
            LoadIncomeData();
        }
    }
}
