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
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        private readonly AccountingModel _database;
        public RegistrationPage()
        {
            InitializeComponent();
            _database = new AccountingModel();
        }

        private void CheckPasswordGenerate(object sender, RoutedEventArgs e)
        {
            PasswordTextBox.Text = AuthorizationService.GeneratePassword();
        }
        private void Registrate_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidationHelper.IsMinLength(LoginTextBox.Text, 5) || !ValidationHelper.IsMinLength(PasswordTextBox.Text, 5))
            {
                MessageBox.Show("Введите логин или пароль больше 5 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!ValidationHelper.IsMaxLength(LoginTextBox.Text, 30) ||!ValidationHelper.IsMaxLength(PasswordTextBox.Text, 80))
            {
                MessageBox.Show("Вы ввели слишиком длиный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!ValidationHelper.IsMaxLength(FirstNameTextBox.Text, 30) || !ValidationHelper.IsMaxLength(LastNameTextBox.Text, 80))
            {
                MessageBox.Show("Вы ввели слишиком длиное имя или фамилию", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string firstName = FirstNameTextBox.Text;
            string lastName = LastNameTextBox.Text;
            string login = LoginTextBox.Text;
            string password = PasswordTextBox.Text;

            if (!_database.Users.All(u => u.Login != login))
            {
                MessageBox.Show("Логин уже существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            User user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Login = login,
                PasswordHash = AuthorizationService.HashPassword(password),
            };

            _database.Users.Add(user);
            _database.SaveChanges();

            MessageBox.Show("Пользователь успешно зарегистрирован!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigationService.Navigate(new LoginPage());
        }

        private void GoBackTextClick(object sender, MouseButtonEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
