using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Animation;
using System.Runtime.Remoting.Contexts;
using System.Data.Entity;

namespace MoneyManagerX.Service
{
    public class UserService
    {
        private User _user;
        private readonly AccountingModel _dbContext;

        public UserService(User user)
        {
            _user = user;
            _dbContext = new AccountingModel();
        }
        public void RefreshAccountsList(ComboBox accountsComboBox, TextBlock balanceText, ListBox incomesListBox, ListBox spendingsListBox)
        {
            var userAccounts = _dbContext.Accounts.Where(a => a.UserId == _user.Id).ToList();
            if (!userAccounts.Any())
            {
                MessageBox.Show("У пользователя нет счетов. Добавьте первый счет", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                accountsComboBox.ItemsSource = null;
                balanceText.Text = "0";
                incomesListBox.ItemsSource = null;
                spendingsListBox.ItemsSource = null;
                return;
            }

            _user.Accounts = userAccounts;
            accountsComboBox.ItemsSource = userAccounts;
            accountsComboBox.SelectedIndex = 0;
        }
        public User GetUserById(int userId)
        {
            return _dbContext.Users.FirstOrDefault(u => u.Id == userId);
        }

        public void RefreshTransaction(int accountId, TextBlock balanceText, ListBox incomesListBox, ListBox spendingsListBox)
        {
            var account = _dbContext.Accounts.AsNoTracking().FirstOrDefault(a => a.Id == accountId);

            balanceText.Text = account?.Balance.ToString() ?? "0";
            incomesListBox.ItemsSource = new TransactionService().GetTransactionsForAccount(accountId, "Доход");
            spendingsListBox.ItemsSource = new TransactionService().GetTransactionsForAccount(accountId, "Расход");

            _user = _dbContext.Users.AsNoTracking().FirstOrDefault(a => a.Id == _user.Id);
        }
        public void EditUser(User user, TextBox LoginTextBox, TextBox FirstNameTextBox, TextBox LastNameTextBox, PasswordBox passwordBox)
        {
            LoginTextBox.Text = _user.Login;
            FirstNameTextBox.Text = _user.FirstName;
            LastNameTextBox.Text = _user.LastName;

            using (_dbContext)
            {
                if (ValidationHelper.IsNullOrWhiteSpace(passwordBox.Password))
                {
                    _dbContext.Users.First(t => t.Login == _user.Login).Login = LoginTextBox.Text;
                    _dbContext.Users.First(t => t.FirstName == _user.FirstName).FirstName = FirstNameTextBox.Text;
                    _dbContext.Users.First(t => t.LastName == _user.LastName).LastName = LastNameTextBox.Text;
                    _dbContext.SaveChanges();

                }
                else
                {
                    _dbContext.Users.First(t => t.Login == _user.Login).Login = LoginTextBox.Text;
                    _dbContext.Users.First(t => t.FirstName == _user.FirstName).FirstName = FirstNameTextBox.Text;
                    _dbContext.Users.First(t => t.LastName == _user.LastName).LastName = LastNameTextBox.Text;
                    _dbContext.Users.First(t => t.Login == _user.Login).PasswordHash = AuthorizationService.HashPassword(passwordBox.Password);                    _dbContext.SaveChanges();
                    _dbContext.SaveChanges();
                }
            }
        }
    }
}
