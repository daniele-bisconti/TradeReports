using System;

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using LiveCharts;
using LiveCharts.Wpf;
using TradeReports.UI.Contracts.ViewModels;
using TradeReports.Core.Interfaces;
using System.Collections.Generic;
using TradeReports.Core.Models;
using System.Linq;
using TradeReports.Core.Types;
using LiveCharts.Defaults;
using TradeReports.Core.Analytics.Interfaces;

namespace TradeReports.UI.ViewModels
{
    public class MainViewModel : ObservableObject, INavigationAware
    {
        private readonly IOperationsAnalysisService _operationsAnalysisService;
        private readonly ICapitalService _capitalService;
        private SeriesCollection _series;
        private List<string> _lables;
        private string _lastCapital;

        public string LastCapital
        {
            get { return _lastCapital; }
            set { SetProperty(ref _lastCapital, value); }
        }



        public List<string> Labels
        {
            get { return _lables; }
            set { SetProperty(ref _lables, value); }
        }


        public SeriesCollection Series
        {
            get { return _series; }
            set 
            { 
                SetProperty(ref _series, value);
            }
        }

        public MainViewModel(ICapitalService capitalService, IOperationsAnalysisService operationsAnalysisService)
        {
            _operationsAnalysisService = operationsAnalysisService;
            _capitalService = capitalService;
        }

        public async void OnNavigatedTo(object parameter)
        {
            Series = new SeriesCollection();
            Labels = new List<string>();

            Series.Add(new LineSeries { Values = new ChartValues<decimal>(), Name = "Media_Mobile" });
            Series.Add(new CandleSeries { Values = new ChartValues<OhlcPoint>(), Name = "Variazione_Capitale"});

            var operations = await _operationsAnalysisService.GetGroupedCapitals(DateTimeAggregation.Day);
            var movingAverage = await _operationsAnalysisService.GetMovingAverage(14);

            foreach (var op in operations)
            {
                Series[1].Values.Add(new OhlcPoint((double)op.Value[0], (double)op.Value.Max(), (double)op.Value.Min(), (double)op.Value[op.Value.Count - 1]));
                Labels.Add(op.Key.ToString("dd/MM/yy"));
            }

            foreach (var ma in movingAverage)
            {
                Series[0].Values.Add(ma.Value);
                Labels.Add(ma.Key.ToString("dd/MM/yy"));
            }

            LastCapital =  _capitalService.GetLastCapital().ToString("#.##");
        }

        public void OnNavigatedFrom()
        {
        }
    }
}
