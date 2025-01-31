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
        private User _user;
        private UserService _userService;
        private AccountService _accountService;

        public HomePage(User user)
        {
            InitializeComponent();
            _user = user;
            _userService = new UserService(_user);
            _accountService = new AccountService();

            AccountsComboBox.ItemsSource = user.Accounts;
            AccountsComboBox.DisplayMemberPath = "Name";
            AccountsComboBox.SelectedValuePath = "Id";
        }
        private void LoadUserData()
        {
            _user = _userService.GetUserById(_user.Id); 
            if (_user != null)
            {
                AccountsComboBox.ItemsSource = _user.Accounts;
                AccountsComboBox.DisplayMemberPath = "Name";
                AccountsComboBox.SelectedValuePath = "Id";
            }
        }

        public void RefreshPage(object sender, RoutedEventArgs e)
        {
            LoadUserData(); 
            _userService.RefreshAccountsList(AccountsComboBox, BalanceText, IncomesListBox, SpendingsListBox);
        }

        private void CreateAccount(object sender, RoutedEventArgs e)
        {
            AccountWindow accountPage = new AccountWindow(_user);
            accountPage.Show();

            _userService.RefreshAccountsList(AccountsComboBox, BalanceText, IncomesListBox, SpendingsListBox);
            LoadUserData();
        }

        private void AccountsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AccountsComboBox.SelectedItem is Account selectedAccount)
            {
                _userService.RefreshTransaction(selectedAccount.Id, BalanceText, IncomesListBox, SpendingsListBox);
            }
            else
            {
                BalanceText.Text = "0";
                IncomesListBox.ItemsSource = null;
                SpendingsListBox.ItemsSource = null;
            }
        }

        private void AddTransaction(object sender, RoutedEventArgs e)
        {
            TransactionWindow transactionWindow = new TransactionWindow(_user);
            transactionWindow.Show();
            LoadUserData();
        }

        private void AddCategory(object sender, RoutedEventArgs e)
        {
            CategoryWindow categoryWindow = new CategoryWindow(_user);
            categoryWindow.Show();
            LoadUserData();
        }
    }
}
