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
using LiveCharts;
using LiveCharts.Wpf;

namespace MoneyManagerX
{
    /// <summary>
    /// Логика взаимодействия для IncomePage.xaml
    /// </summary>
    public partial class IncomePage : Page
    {
        private readonly AccountingModel _dbcontext;

        public IncomePage(User _user)
        {

            InitializeComponent();
            pieChart.Series = new SeriesCollection()
            {
                 new PieSeries
            {
                Values = new ChartValues<double> {40 },
                DataLabels = true,
                Fill = new SolidColorBrush(Colors.Green),
                LabelPoint = chartPoint => $"{chartPoint.Y}({chartPoint.Participation:P})"

            },
            new PieSeries
            {
                Values = new ChartValues<double> { 20 },
                DataLabels = true,
                Fill = new SolidColorBrush(Colors.Blue),
                LabelPoint = chartPoint => $"{chartPoint.Y}({chartPoint.Participation:P})"

            },
            new PieSeries
            {
                Values = new ChartValues<double> { 40 },
                DataLabels = true,
                Fill = new SolidColorBrush(Colors.Red),
                LabelPoint = chartPoint => $"{chartPoint.Y}({chartPoint.Participation:P})"

            }
            
        };
            pieChart.LegendLocation = LegendLocation.Right;

        }
    }
}
