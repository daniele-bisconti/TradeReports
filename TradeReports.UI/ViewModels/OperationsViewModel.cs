using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
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
        private readonly IOperationsServiceAsync _operationsService;
        private readonly INavigationService _navigationService;
        private readonly IDialogCoordinator _dialogCoordinator;

        private RelayCommand _addOperation;
        private RelayCommand _deleteOperation;
        private RelayCommand _filterChanged;

        public ICommand FilterChanged => _filterChanged ?? (_filterChanged = new RelayCommand(OnFilterChanged));
        private async void OnFilterChanged()
        {
            await RefreshOperationsList();
        }

        public ObservableCollection<Operation> Operations { get; } = new ObservableCollection<Operation>();
        public ObservableCollection<Operation> VisualizedOperations { get; } = new ObservableCollection<Operation>();

        private Operation _selectedOperation;


        public Operation SelectedOperation
        {
            get { return _selectedOperation; }
            set 
            {
                SetProperty(ref _selectedOperation, value);
                (DeleteOperation as RelayCommand).NotifyCanExecuteChanged();
            }
        }

        #region Pagination

        private int _numOfPages;

        public int NumOfPages
        {
            get { return _numOfPages; }
            set 
            { 
                SetProperty(ref _numOfPages, value);
            }
        }

        private int _currentPage;

        public int CurrentPage
        {
            get { return _currentPage; }
            set 
            {
                SetProperty(ref _currentPage, value);
            }
        }


        private int _elementPerPage;


        public int ElementPerPage
        {
            get { return _elementPerPage; }
            set 
            { 
                var val = value == 0 ? 1: value;
                SetProperty(ref _elementPerPage, val);
                NumOfPages = Operations.Count / ElementPerPage;
                CurrentPage = 1;
                SetVisualizedOperations();
            }
        }


        private RelayCommand _nextPage;
        private RelayCommand _prevPage;
        private RelayCommand _firstPage;
        private RelayCommand _lastPage;

        public ICommand NextPage => _nextPage ?? (_nextPage = new RelayCommand(OnNextPage));
        public ICommand PrevPage => _prevPage ?? (_prevPage = new RelayCommand(OnPrevPage));
        public ICommand FirstPage => _firstPage ?? (_firstPage = new RelayCommand(OnFirstPage));
        public ICommand LastPage => _lastPage ?? (_lastPage = new RelayCommand(OnLastPage));

        private void OnNextPage()
        {
            CurrentPage = CurrentPage == NumOfPages ? 1 : CurrentPage + 1;
            SetVisualizedOperations();
        }

        private void OnPrevPage()
        {
            CurrentPage = CurrentPage == 1 ? NumOfPages : CurrentPage - 1;
            SetVisualizedOperations();
        }

        private void OnFirstPage()
        {
            CurrentPage = 1;
            SetVisualizedOperations();
        }

        private void OnLastPage()
        {
            CurrentPage = NumOfPages;
            SetVisualizedOperations();
        }
        #endregion

        #region Filters

        public ObservableCollection<int> Years { get; set; } = new ObservableCollection<int>();

        private int _selectedYear;

        public int SelectedYear
        {
            get { return _selectedYear; }
            set 
            {
                SetProperty(ref _selectedYear, value);
            }
        }

        public ObservableCollection<string> Month {  get ; set; } = new ObservableCollection<string>();

        private string _selectedMonth;

        public string SelectedMonth
        {
            get { return _selectedMonth; }
            set 
            {
                SetProperty(ref _selectedMonth, value);
                RefreshOperationsList().Wait();
            }
        }


        private int _day;
        public int Day
        {
            get { return _day; }
            set 
            {
                SetProperty(ref _day, value);
                RefreshOperationsList().Wait();
            }
        }

        #endregion

        public OperationsViewModel(IOperationsServiceAsync dataService, INavigationService navigationService)
        {
            _operationsService = dataService;
            _navigationService = navigationService;
            _dialogCoordinator = DialogCoordinator.Instance;
        }

        public async void OnNavigatedTo(object parameter)
        {
            _elementPerPage = 20;
            CurrentPage = 1;

            // Imposta filtro anno
            Years = new ObservableCollection<int>( await _operationsService.GetOperationsYears());
            SelectedYear = Years.FirstOrDefault(y => y == DateTime.Now.Year);

            // Imposta filtro mese
            Month = new ObservableCollection<string>( DateTimeFormatInfo.CurrentInfo.MonthNames );
            SelectedMonth = Month.FirstOrDefault(m => DateTimeFormatInfo.CurrentInfo.MonthNames.ToList().IndexOf(m) + 1 == DateTime.Now.Month);
            
            await RefreshOperationsList();
        }

        private static int MonthStringToNumber(string month)
        {
            return DateTimeFormatInfo.CurrentInfo.MonthNames.ToList().IndexOf(month) + 1;
        }

        private async Task RefreshOperationsList()
        {
            try
            {
                Operations.Clear();

                // Replace this with your actual data
                var data = await _operationsService.GetOperationsAsync(SelectedYear, MonthStringToNumber(SelectedMonth));
                data = data.OrderByDescending(o => o.CloseDate);

                foreach (var item in data)
                {
                    Operations.Add(item);
                }
                
                SetVisualizedOperations();
            }
            catch (Exception e)
            {
                Log.Logger.Error(e.ToString());
                throw;
            }
            
        }

        private void SetVisualizedOperations()
        {
            var ops = Operations
                    .Skip(ElementPerPage * (CurrentPage - 1))
                    .Take(ElementPerPage);

            int opsNum = Operations.Count() == 0 ? 1 : Operations.Count();
            NumOfPages = opsNum / ElementPerPage;
            NumOfPages = NumOfPages == 0 ? 1 : NumOfPages;

            CurrentPage = CurrentPage > NumOfPages ? NumOfPages : CurrentPage;

            VisualizedOperations.Clear();

            foreach(var op in ops)
            {
                VisualizedOperations.Add(op);
            }
        }

        public void OnNavigatedFrom()
        {

        }

        public ICommand AddOperation => _addOperation ?? (_addOperation = new RelayCommand(OnAddOperationInvoked));
        public ICommand DeleteOperation => _deleteOperation ?? (_deleteOperation = new RelayCommand(OnDeleteOperationInvoked, CanRemove));

        private void OnAddOperationInvoked()
            => _navigationService.NavigateTo(typeof(AddOperationViewModel).FullName);

        private async void OnDeleteOperationInvoked()
        {
            Operation selected = SelectedOperation;

            try
            {
                MessageDialogResult result = await _dialogCoordinator.ShowMessageAsync(this, "Conferma eliminazione", $"Eliminare l'operazione aperta il: {selected.OpenDate} e chiusa il: {selected.CloseDate}", MessageDialogStyle.AffirmativeAndNegative);

                if (result == MessageDialogResult.Affirmative)
                {
                    await _operationsService.DeleteOperationAsync(selected.Id.ToString());
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
