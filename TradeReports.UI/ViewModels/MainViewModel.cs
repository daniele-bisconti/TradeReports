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
using TradeReports.Core.Analitycs.Capital;
using System.Collections.ObjectModel;

namespace TradeReports.UI.ViewModels
{
    public class MainViewModel : ObservableObject, INavigationAware
    {
        private const decimal NEXT_TRADE_MULTIPLIER = 0.005m;
        private readonly IOperationsAnalysisService _operationsAnalysisService;
        private readonly ICapitalService _capitalService;
        private readonly IOperationsServiceAsync _operationsService;
        private string _lastCapital;
        private string _nextTrade;
        private string _marginSize;
        private int _margin;

        public ObservableCollection<int> Margins { get; set; }

        public string LastCapital
        {
            get { return _lastCapital; }
            set { SetProperty(ref _lastCapital, value); }
        }

        public string NextTrade
        {
            get { return _nextTrade; }
            set { SetProperty(ref _nextTrade, value); }
        }

        public int Margin
        {
            get => _margin;
            set => SetMargin(value);
        }

        public string MarginSize
        {
            get => _marginSize;
            set => SetProperty(ref _marginSize, value);
        }

        private void SetMargin(int value)
        {
            _margin = value;
            MarginSize = (Convert.ToDouble(LastCapital) / value).ToString("##.##");
        }

        private CapitalVariationChartViewModel _chartViewModel;

        public CapitalVariationChartViewModel ChartViewModel
        {
            get { return _chartViewModel; }
            set { SetProperty(ref _chartViewModel, value); }
        }

        public MainViewModel(ICapitalService capitalService, IOperationsAnalysisService operationsAnalysisService, IOperationsServiceAsync operationsService)
        {
            _operationsAnalysisService = operationsAnalysisService;
            _capitalService = capitalService;
            _operationsService = operationsService;

            Margins = new ObservableCollection<int>();

            for(int i = 4; i <= 10; i++)
            {
                Margins.Add(i);
            }
        }

        public async void OnNavigatedTo(object parameter)
        {
            var ops = (await _operationsService.GetGridDataAsync()).ToList();
            CapitalAnalysis capAn = new CapitalAnalysis(ops);
            ChartViewModel = new CapitalVariationChartViewModel(capAn);


            LastCapital =  _capitalService.GetLastCapital().ToString("F2");
            NextTrade = (Convert.ToDecimal(LastCapital) * NEXT_TRADE_MULTIPLIER).ToString("F2");
            Margin = 5;
        }

        public void OnNavigatedFrom()
        {
        }
    }
}
