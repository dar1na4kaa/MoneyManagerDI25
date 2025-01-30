using MoneyManagerX.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

namespace MoneyManagerX
{
    /// <summary>
    /// Логика взаимодействия для HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        private readonly User _user;
        private readonly TransactionService transactionService;
        public HomePage(User user)
        {
            InitializeComponent();
            _user = user;
            transactionService = new TransactionService();

            AccountsComboBox.ItemsSource = _user.Accounts;
            AccountsComboBox.DisplayMemberPath = "Name";
            AccountsComboBox.SelectedValuePath = "Id";

        }

        private void RefreshImageClick(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new HomePage(_user));
        }


        private void CreateAccount(object sender, RoutedEventArgs e)
        {
            AccountWindow accountWindow = new AccountWindow(_user);
            accountWindow.Show();
        }


        private void AddTransaction(object sender, RoutedEventArgs e)
        {
            var transactionWin = new TransactionWindow(_user);
            transactionWin.Show();
        }

        public void RefreshPage(object sender, RoutedEventArgs e)
        {
            RefreshAccountsList();
        }

        private void RefreshAccountsList()
        {
            using (var dbContext = new AccountingModel())
            {
                var userAccounts = dbContext.Accounts.Where(a => a.UserId == _user.Id).ToList();
                if (!userAccounts.Any())
                {
                    MessageBox.Show("У пользователя нет счетов. Добавьте первый счет", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    AccountsComboBox.ItemsSource = null;
                    BalanceText.Text = "0";
                    IncomesListBox.ItemsSource = null;
                    SpendingsListBox.ItemsSource = null;
                    return;
                }

                AccountsComboBox.ItemsSource = userAccounts;
                AccountsComboBox.SelectedIndex = 0; 
            }
        }

        private void AccountsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (var dbContext = new AccountingModel())
            {
                if (AccountsComboBox.SelectedItem is Account selectedAccount)
                {
                    RefreshTransaction(selectedAccount.Id);
                }
                else
                {
                    BalanceText.Text = "0";
                    IncomesListBox.ItemsSource = null;
                    SpendingsListBox.ItemsSource = null;
                }
            }
        }

        private void RefreshTransaction(int accountId)
        {
            using (var dbContext = new AccountingModel())
            {
                var account = dbContext.Accounts.AsNoTracking().FirstOrDefault(a => a.Id == accountId);

                BalanceText.Text = account?.Balance.ToString() ?? "0";
                IncomesListBox.ItemsSource = transactionService.GetIncomeForAccount(accountId);
                SpendingsListBox.ItemsSource = transactionService.GetSpendingForAccount(accountId);
            }
        }

        private void AddCategory(object sender, RoutedEventArgs e)
        {
            CategoryWindow categoryWindow = new CategoryWindow(_user);
            categoryWindow.Show();
        }
    }
}
