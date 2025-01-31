using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveCharts.Wpf;
using LiveCharts;

namespace MoneyManagerX.Service
{
    public class ChartService
    {
        public void UpdatePieChart(PieChart pieChart, IEnumerable<Transaction> incomes)
        {
            pieChart.Series.Clear();

            foreach (var income in incomes)
            {
                pieChart.Series.Add(new PieSeries
                {
                    Values = new ChartValues<decimal> { income.Amount },
                    Title = income.Category.Name,
                    DataLabels = true
                });
            }
        }
    }
}
