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
using System.Windows.Shapes;

namespace MoneyManagerX
{
    /// <summary>
    /// Логика взаимодействия для ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        private readonly User _user;
        public ManagerWindow(User user)
        {
            InitializeComponent();
            _user = user;
            ManagerFrame.Navigate(new HomePage(_user));
        }

        private void HomeImageClick(object sender, MouseButtonEventArgs e)
        {
            ManagerFrame.Navigate(new HomePage(_user));
        }

        private void IncomeImageClick(object sender, MouseButtonEventArgs e)
        {
            ManagerFrame.Navigate(new IncomePage(_user));
        }

        private void SpendingsImageClick(object sender, MouseButtonEventArgs e)
        {
            ManagerFrame.Navigate(new SpendingsPage(_user));
        }

        private void AccountImageClick(object sender, MouseButtonEventArgs e)
        {
            ManagerFrame.Navigate(new AccountPage(_user));
        }
    }
}
