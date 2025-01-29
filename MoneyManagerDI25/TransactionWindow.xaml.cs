using MoneyManagerX.Service;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Логика взаимодействия для IncomeWindow.xaml
    /// </summary>
    public partial class TransactionWindow : Window
    {
        TransactionService transactionService;
        public TransactionWindow(User user)
        {
            InitializeComponent();
            transactionService = new TransactionService();

            AccountBox.ItemsSource = user.Accounts;
            CategoriesComboBox.DisplayMemberPath = "Name";
            CategoriesComboBox.SelectedValuePath = "Id";

            CategoriesComboBox.ItemsSource = user.Categories;
            CategoriesComboBox.DisplayMemberPath = "Name"; 
            CategoriesComboBox.SelectedValuePath = "Id";  
        }

        private void AddTransaction(object sender, RoutedEventArgs e)
        {
            if (AccountBox.SelectedItem == null || CategoriesComboBox.SelectedItem == null)
            {
                MessageBox.Show("Необходимо выбрать счет и категорию.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!decimal.TryParse(AmountBox.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal amount))
            {
                MessageBox.Show("Некорректный формат суммы.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(DescriptionBox.Text))
            {
                MessageBox.Show("Описание не может быть пустым", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (DatePicker.SelectedDate == null)
            {
                MessageBox.Show("Необходимо выбрать дату.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (TypeBox.SelectedItem == null)
            {
                MessageBox.Show("Необходимо выбрать тип транзакции.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            try
            {
                var selectedAccount = (Account)AccountBox.SelectedItem;
                var selectedCategory = (Category)CategoriesComboBox.SelectedItem;
                var transactionType = (ComboBoxItem)TypeBox.SelectedItem;

                var transaction = new Transaction
                {
                    AccountId = selectedAccount.Id,
                    CategoryId = selectedCategory.Id,
                    Date = DatePicker.SelectedDate.Value,
                    Description = DescriptionBox.Text,
                    Amount = amount,
                    Type = transactionType.Content.ToString()
                };
                using (var _dbcontext = new AccountingModel())
                {
                    _dbcontext.Transactions.Add(transaction);
                }
                MessageBox.Show("Транзакция успешно добавлена.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                transactionService.RecalculateBalance(selectedAccount.Id, transactionType.Content.ToString(), amount);
                DescriptionBox.Clear();
                AmountBox.Clear();
                DatePicker.SelectedDate = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении транзакции: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
