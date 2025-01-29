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

            Account acc = _user.Accounts.First(x => x.Id == 1);
            BalanceText.Text = acc.Balance.ToString();

            IncomesListBox.ItemsSource = transactionService.GetIncomeForUser(_user.Id);

            SpendingsListBox.ItemsSource = transactionService.GetSpendingForUser(_user.Id);
        }

        private void RefreshImageClick(object sender, MouseButtonEventArgs e)
        {
          NavigationService.Navigate(new HomePage(_user));
        }

        private void CreateAccount(object sender, RoutedEventArgs e)
        {

        }

        private void AddTransaction(object sender, RoutedEventArgs e)
        {
            var transactionWin = new TransactionWindow(_user);
            transactionWin.Show();
        }

        private void RefreshPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Refresh();
        }
    }
}
