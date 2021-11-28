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
        private readonly IEnumerable<Operation> _operations;

        private ShortLongReport _shortLongReport;
        public ShortLongReport ShortLongReport
        {
            get { return _shortLongReport; }
            set
            {
                SetProperty(ref _shortLongReport, value);
            }
        }

        public SeriesCollection ShortSeries { get; set; } = new SeriesCollection();
        public SeriesCollection LongSeries { get; set; } = new SeriesCollection();
        public SeriesCollection ShortAmountSeries { get; set; } = new SeriesCollection();
        public SeriesCollection LongAmountSeries { get; set; } = new SeriesCollection();

        private DateTime? _startDate;

        public DateTime? StartDate
        {
            get { return _startDate; }
            set
            {
                SetProperty(ref _startDate, value);
                UpdateCharts();
            }
        }


        private DateTime? _endDate;

        public DateTime? EndDate
        {
            get { return _endDate; }
            set
            {
                SetProperty(ref _endDate, value);
                UpdateCharts();
            }
        }

        public ShortLongReportViewModel(IEnumerable<Operation> operations)
        {
            _operations = operations;

            _startDate = DateTime.Today;
            _endDate = DateTime.Today.AddDays(1).AddTicks(-1); 
            UpdateCharts();
        }

        private void UpdateCharts()
        {
            ShortLongReport = new ShortLongReport(_operations.Where(op => op.CloseDate >= StartDate && op.CloseDate <= EndDate));
            SetShortSeries();

            SetLongSeries();

            SetShortAmountSeries();

            SetLongAmountSeries();
        }

        private void SetShortAmountSeries()
        {
            if (!ShortAmountSeries.Any())
            {
                ShortAmountSeries.Add(new PieSeries
                {
                    Title = "€ Loss",
                    Values = new ChartValues<ObservableValue> { new ObservableValue((double)ShortLongReport.AmountOfShortLoss) },
                    DataLabels = true,
                    Fill = Brushes.Red,
                    FontSize = 14
                });

                ShortAmountSeries.Add(new PieSeries
                {
                    Title = "€ Profit",
                    Values = new ChartValues<ObservableValue> { new ObservableValue((double)ShortLongReport.AmountOfShortProfit) },
                    DataLabels = true,
                    Fill = Brushes.Green,
                    FontSize = 14
                });
            }
            else
            {
                ((ObservableValue)ShortAmountSeries[0].Values[0]).Value = (double)ShortLongReport.AmountOfShortLoss;
                ((ObservableValue)ShortAmountSeries[1].Values[0]).Value = (double)ShortLongReport.AmountOfShortProfit;
            }
        }

        private void SetLongAmountSeries()
        {
            if (!LongAmountSeries.Any())
            {
                LongAmountSeries.Add(new PieSeries
                {
                    Title = "€ Loss",
                    Values = new ChartValues<ObservableValue> { new ObservableValue((double)ShortLongReport.AmountOfLongLoss) },
                    DataLabels = true,
                    Fill = Brushes.Red,
                    FontSize = 14
                });

                LongAmountSeries.Add(new PieSeries
                {
                    Title = "€ Profit",
                    Values = new ChartValues<ObservableValue> { new ObservableValue((double)ShortLongReport.AmountOfLongProfit) },
                    DataLabels = true,
                    Fill = Brushes.Green,
                    FontSize = 14
                });
            }
            else
            {
                ((ObservableValue)LongAmountSeries[0].Values[0]).Value = (double)ShortLongReport.AmountOfLongLoss;
                ((ObservableValue)LongAmountSeries[1].Values[0]).Value = (double)ShortLongReport.AmountOfLongProfit;
            }
        }

        private void SetLongSeries()
        {
            if (!LongSeries.Any())
            {
                LongSeries.Add(new PieSeries
                {
                    Title = "% Loss",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(ShortLongReport.LongLossPercentage) },
                    DataLabels = true,
                    Fill = Brushes.Red,
                    FontSize = 14
                });

                LongSeries.Add(new PieSeries
                {
                    Title = "% Profit",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(ShortLongReport.LongProfitPercentage) },
                    DataLabels = true,
                    Fill = Brushes.Green,
                    FontSize = 14
                });
            }
            else
            {
                ((ObservableValue)LongSeries[0].Values[0]).Value = (double)ShortLongReport.NumOfLongLoss;
                ((ObservableValue)LongSeries[1].Values[0]).Value = (double)ShortLongReport.NumOfLongProfit;
            }
        }

        private void SetShortSeries()
        {
            if (!ShortSeries.Any())
            {
                ShortSeries.Add(new PieSeries
                {
                    Title = "% Loss",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(ShortLongReport.ShortLossPercentage) },
                    DataLabels = true,
                    Fill = Brushes.Red,
                    FontSize = 14
                });

                ShortSeries.Add(new PieSeries
                {
                    Title = "% Profit",
                    Values = new ChartValues<ObservableValue> { new ObservableValue(ShortLongReport.ShortProfitPercentage) },
                    DataLabels = true,
                    Fill = Brushes.Green,
                    FontSize = 14
                });
            }
            else
            {
                ((ObservableValue)ShortSeries[0].Values[0]).Value = (double)ShortLongReport.NumOfShortLoss;
                ((ObservableValue)ShortSeries[1].Values[0]).Value = (double)ShortLongReport.NumOfShortProfit;
            }
        }
    }
}
