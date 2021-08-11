using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using TradeReports.Core.Interfaces;
using TradeReports.Core.Models;
using TradeReports.Core.Repository;
using TradeReports.Core.Services;
using TradeReports.UI.Contracts.Services;
using TradeReports.UI.Contracts.ViewModels;
using TradeReports.UI.Core.Contracts.Services;
using TradeReports.UI.Core.Models;

namespace TradeReports.UI.ViewModels
{
    public class OperationsViewModel : ObservableObject, INavigationAware
    {
        private readonly IOperationsServiceAsync _dataService;
        private readonly INavigationService _navigationService;

        private RelayCommand _addOperation;
        public ObservableCollection<Operation> Source { get; } = new ObservableCollection<Operation>();

        public OperationsViewModel(IOperationsServiceAsync dataService, INavigationService navigationService)
        {
            _dataService = dataService;
            _navigationService = navigationService;
        }

        public async void OnNavigatedTo(object parameter)
        {
            Source.Clear();

            // Replace this with your actual data
            var data = await _dataService.GetGridDataAsync();

            foreach (var item in data)
            {
                Source.Add(item);
            }
        }

        public void OnNavigatedFrom()
        {
        }

        public ICommand AddOperation => _addOperation ?? (_addOperation = new RelayCommand(OnAddOperationInvoked));

        private void OnAddOperationInvoked()
            => _navigationService.NavigateTo(typeof(AddOperationViewModel).FullName);
    }
}
