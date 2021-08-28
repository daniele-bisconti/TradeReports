using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Serilog;
using TradeReports.Core.Interfaces;
using TradeReports.Core.Models;
using TradeReports.Core.Repository;
using TradeReports.Core.Services;
using TradeReports.UI.Contracts.Services;
using TradeReports.UI.Contracts.ViewModels;

namespace TradeReports.UI.ViewModels
{
    public class OperationsViewModel : ObservableObject, INavigationAware
    {
        private readonly IOperationsServiceAsync _dataService;
        private readonly INavigationService _navigationService;
        private readonly IDialogCoordinator _dialogCoordinator;

        private RelayCommand _addOperation;
        private RelayCommand _deleteOperation;

        public ObservableCollection<Operation> Source { get; } = new ObservableCollection<Operation>();

        private Operation _selectedOperation;

        public Operation SelectedOperation
        {
            get { return _selectedOperation; }
            set 
            {
                SetProperty(ref _selectedOperation, value);
            }
        }

        public OperationsViewModel(IOperationsServiceAsync dataService, INavigationService navigationService)
        {
            _dataService = dataService;
            _navigationService = navigationService;
            _dialogCoordinator = DialogCoordinator.Instance;
        }

        public async void OnNavigatedTo(object parameter)
        {
            await RefreshOperationsList();
        }

        private async Task RefreshOperationsList()
        {
            try
            {
                Source.Clear();

                // Replace this with your actual data
                var data = await _dataService.GetGridDataAsync();
                data = data.OrderBy(o => o.CloseDate);

                foreach (var item in data)
                {
                    Source.Add(item);
                }

            }
            catch (Exception e)
            {
                Log.Logger.Error(e.ToString());
                throw;
            }
        }

        public void OnNavigatedFrom()
        {
        }

        public ICommand AddOperation => _addOperation ?? (_addOperation = new RelayCommand(OnAddOperationInvoked));
        public ICommand DeleteOperation => _deleteOperation ?? (_deleteOperation = new RelayCommand(OnDeleteOperationInvoked));

        private void OnAddOperationInvoked()
            => _navigationService.NavigateTo(typeof(AddOperationViewModel).FullName);

        private async void OnDeleteOperationInvoked()
        {
            Operation selected = SelectedOperation;

            if (!CanRemove()) return;

            try
            {
                MessageDialogResult result = await _dialogCoordinator.ShowMessageAsync(this, "Conferma eliminazione", $"Eliminare l'operazione aperta il: {selected.OpenDate} e chiusa il: {selected.CloseDate}", MessageDialogStyle.AffirmativeAndNegative);

                if (result == MessageDialogResult.Affirmative)
                {
                    await _dataService.DeleteOperationAsync(selected.Id.ToString());
                    await RefreshOperationsList();
                    Log.Logger.Information($"Removed operation with id {selected.Id}");
                }
            }
            catch(Exception e)
            {
                Log.Logger.Error(e.ToString());
                throw;
            }
        }

        private bool CanRemove()
        {
            return SelectedOperation != null;
        }
    }
}
