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
using System.Windows.Shapes;

namespace MoneyManagerX
{
    /// <summary>
    /// Логика взаимодействия для AccountWindowxaml.xaml
    /// </summary>
    public partial class AccountWindow : Window
    {
        private readonly User _user;
        private readonly AccountService accountService;
        public AccountWindow(User user)
        {
            InitializeComponent();
            _user = user;
            accountService = new AccountService();
        }
        private void AddAccount(object sender, RoutedEventArgs e)
        {
            string accountName = AccountNameBox.Text;

            if (string.IsNullOrWhiteSpace(accountName))
            {
                MessageBox.Show("Наименование счета не может быть пустым.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!decimal.TryParse(BalanceBox.Text, out decimal balance) || balance < 0)
            {
                MessageBox.Show("Введите корректный баланс.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            accountService.AddAccount(accountName, balance,_user.Id);

            MessageBox.Show($"Счет \"{accountName}\" успешно добавлен с балансом {balance}.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            AccountNameBox.Clear();
            BalanceBox.Clear();
        }
    }
}
