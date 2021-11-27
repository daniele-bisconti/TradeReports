using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using TradeReports.Core.Analitycs.Models;
using TradeReports.Core.Interfaces;
using TradeReports.Core.Models;

namespace TradeReports.UI.ViewModels
{
    public class ShortLongReportViewModel : ObservableObject
    {
        public ShortLongReport ShortLongReport { get; set; }

        public SeriesCollection ShortSeries { get; set; } = new SeriesCollection();
        public SeriesCollection LongSeries { get; set; } = new SeriesCollection();

        public ShortLongReportViewModel(IEnumerable<Operation> operations)
        {
            ShortLongReport = new ShortLongReport(operations);

            ShortSeries.Add(new PieSeries
            {
                Title = "Short Loss",
                Values = new ChartValues<ObservableValue> { new ObservableValue(ShortLongReport.ShortLossPercentage) },
                DataLabels = true,
                Fill = Brushes.Red,
                FontSize = 14
            });

            ShortSeries.Add(new PieSeries
            {
                Title = "Short Profit",
                Values = new ChartValues<ObservableValue> { new ObservableValue(ShortLongReport.ShortProfitPercentage) },
                DataLabels = true,
                Fill = Brushes.Green,
                FontSize = 14
            });

            LongSeries.Add(new PieSeries
            {
                Title = "Long Loss",
                Values = new ChartValues<ObservableValue> { new ObservableValue(ShortLongReport.LongLossPercentage) },
                DataLabels = true,
                Fill = Brushes.Red,
                FontSize = 14
            });

            LongSeries.Add(new PieSeries
            {
                Title = "Long Profit",
                Values = new ChartValues<ObservableValue> { new ObservableValue(ShortLongReport.LongProfitPercentage) },
                DataLabels = true,
                Fill = Brushes.Green,
                FontSize = 14
            });

        }
    }
}
