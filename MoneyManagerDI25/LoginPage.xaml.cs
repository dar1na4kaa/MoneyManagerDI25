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
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private readonly AccountingModel _dbcontext;
        public LoginPage()
        {
            InitializeComponent();
            _dbcontext = new AccountingModel();
            
        }
        private void Login_Button_Click(object sender, RoutedEventArgs e)
        {
            if (ValidationHelper.IsNullOrWhiteSpace(LoginTextBox.Text) || ValidationHelper.IsNullOrWhiteSpace(PasswordTextBox.Password))
            {
                MessageBox.Show("Вы не ввели логин или пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string login = LoginTextBox.Text;
            string password = PasswordTextBox.Password;
            User user = _dbcontext.Users.FirstOrDefault(u => u.Login == login);

            if (user == null || !AuthorizationService.VerifyPassword(password, user.PasswordHash))
            {
                MessageBox.Show("Неверный логин или пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("Вы успешно вошли в систему!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            ManagerWindow window = new ManagerWindow(user);
            window.Show();
        }
        private void RegistrateTextLink(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new RegistrationPage(_dbcontext));
        }
    }
}
