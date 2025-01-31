using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MoneyManagerX.Service;

namespace MoneyManagerX
{
    /// <summary>
    /// Логика взаимодействия для SpendingsPage.xaml
    /// </summary>
    public partial class SpendingsPage : Page
    {
        private ChartService _chartService;
        private TransactionService _transactionService;
        private CategoryService _categoryService;
        private User _user;
        private DateTime _startDate;
        private DateTime _endDate;
        private int? _selectedAccountId;

        public SpendingsPage(User user)
        {
            InitializeComponent();
            _user = user;
            _transactionService = new TransactionService();
            _chartService = new ChartService();
            _categoryService = new CategoryService();
            LoadAccounts();
            SetDefaultPeriod();
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
                TotalSpendingBlock.Text = _transactionService.GetAllTransactionBalance(_selectedAccountId.Value, "Расход") + " рублей";

                var incomes = _transactionService.GetTransactionsForUser(_selectedAccountId.Value, _startDate, _endDate,"Расход");

                _chartService.UpdatePieChart(pieChart, incomes);
            }
        }



        private void OnAddSpendingClick(object sender, RoutedEventArgs e)
        {
            TransactionWindow transactionWindow = new TransactionWindow(_user);
            transactionWindow.Show();
        }
    }
}

