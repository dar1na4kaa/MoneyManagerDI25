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
        private readonly AccountingModel _dbcontext;
        private readonly User _user;
        public HomePage(User user)
        {
            InitializeComponent();
            _user = user;
            _dbcontext = new AccountingModel();

        }

        private void RefreshImageClick(object sender, MouseButtonEventArgs e)
        {
          NavigationService.Navigate(new HomePage(_user));
        }

        private void CreateTransaction(object sender, RoutedEventArgs e)
        {

        }

        private void AddIncome(object sender, RoutedEventArgs e)
        {
            
        }

        private void AddSpending(object sender, RoutedEventArgs e)
        {

        }
    }
}
