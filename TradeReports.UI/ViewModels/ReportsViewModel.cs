using System;

using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using TradeReports.Core.Interfaces;
using TradeReports.UI.Contracts.ViewModels;

namespace TradeReports.UI.ViewModels
{
    public class ReportsViewModel : ObservableObject, INavigationAware
    {
        private readonly IOperationsServiceAsync _operationsService;
        public ShortLongReportViewModel ShortLongReportViewModel { get; set; }

        public ReportsViewModel(IOperationsServiceAsync operationsService)
        {
            this._operationsService = operationsService;
        }

        public async void OnNavigatedTo(object parameter)
        {
            ShortLongReportViewModel = new ShortLongReportViewModel(await _operationsService.GetOperationsAsync());
        }

        public void OnNavigatedFrom()
        {
        }
    }
}
