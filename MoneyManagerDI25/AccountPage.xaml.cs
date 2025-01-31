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
    /// Логика взаимодействия для AccountPage.xaml
    /// </summary>
    public partial class AccountPage : Page
    {
        private User _user;
        private UserService _userService;

        public AccountPage(User user)
        {
            InitializeComponent();
            _user = user;
            _userService = new UserService(_user);
            DisplayUserInfo();
        }

        private void DisplayUserInfo()
        {
            LoginTextBlock.Text = _user.Login;
            FirstNameTextBlock.Text = _user.FirstName;
            LastNameTextBlock.Text = _user.LastName;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            EditPanel.Visibility = Visibility.Visible;
            InfoPanel.Visibility = Visibility.Hidden;

            LoginTextBox.Text = _user.Login;
            FirstNameTextBox.Text = _user.FirstName;
            LastNameTextBox.Text = _user.LastName;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string newPassword = NewPasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (!ValidationHelper.IsMinLength(LoginTextBox.Text, 5) || !ValidationHelper.IsMinLength(NewPasswordBox.Password, 5))
            {
                MessageBox.Show("Введите логин или н пароль больше 5 символов", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!ValidationHelper.IsMaxLength(LoginTextBox.Text, 30) || !ValidationHelper.IsMaxLength(NewPasswordBox.Password, 80))
            {
                MessageBox.Show("Вы ввели слишиком длиный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!ValidationHelper.IsMaxLength(FirstNameTextBox.Text, 30) || !ValidationHelper.IsMaxLength(LastNameTextBox.Text, 80))
            {
                MessageBox.Show("Вы ввели слишиком длиное имя или фамилию", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (ValidationHelper.IsNullOrWhiteSpace(LoginTextBox.Text) || ValidationHelper.IsNullOrWhiteSpace(FirstNameTextBox.Text)
                || ValidationHelper.IsNullOrWhiteSpace(LastNameTextBox.Text))
            {
                MessageBox.Show("Вы ввели не все данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

            }

            if (NewPasswordBox.Password.Equals(ConfirmPasswordBox.Password) || (ValidationHelper.IsNullOrWhiteSpace(NewPasswordBox.Password) && ValidationHelper.IsNullOrWhiteSpace(ConfirmPasswordBox.Password)))
            {
                _userService.EditUser(_user, LoginTextBox, FirstNameTextBox, LastNameTextBox, NewPasswordBox);

                MessageBox.Show("Данные успешно изменены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                InfoPanel.Visibility = Visibility.Visible;
                EditPanel.Visibility = Visibility.Collapsed;
                NewPasswordBox.Clear();
                ConfirmPasswordBox.Clear();

                DisplayUserInfo();

            }
            else
            {
                MessageBox.Show("Пароли не совпадают", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();

            var parentWindow = Window.GetWindow(this);
            parentWindow.Close();
        }
    }
}

