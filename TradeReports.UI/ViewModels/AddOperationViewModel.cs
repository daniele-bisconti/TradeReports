using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TradeReports.Core.Interfaces;
using TradeReports.Core.Models;
using TradeReports.Core.Models.Params;
using TradeReports.UI.Contracts.Services;
using TradeReports.UI.Contracts.ViewModels;

namespace TradeReports.UI.ViewModels
{
    public class AddOperationViewModel : ObservableObject, INavigationAware
    {
        private AddOperationParams _operation = new AddOperationParams();
        private decimal _capitalDT;
        private string _categoryText;
        private string _toolText;

        private RelayCommand _addCategory;
        private RelayCommand _addTool;
        private RelayCommand _addOperation;

        private readonly ICapitalServiceAsync _capitalService;
        private readonly ICategoryServiceAsync _categoryService;
        private readonly IPosServiceAsync _posService;
        private readonly IOperationsServiceAsync _operationService;
        private readonly INavigationService _navigationService;
        public AddOperationViewModel(ICapitalServiceAsync capitalService, 
            ICategoryServiceAsync categoryService, 
            IPosServiceAsync posService, 
            IOperationsServiceAsync operationsService,
            INavigationService navigationService)
        {
            _capitalService = capitalService;
            _categoryService = categoryService;
            _posService = posService;
            _operationService = operationsService;
            _navigationService = navigationService;
        }

        public DateTime OpenDate
        {
            get => _operation.OpenDate;
            set
            {
                _operation.OpenDate = value;
                OnPropertyChanged(nameof(OpenDate));
            }
        }

        public DateTime CloseDate
        {
            get => _operation.CloseDate;
            set
            {
                _operation.CloseDate = value;
                OnPropertyChanged(nameof(CloseDate));
            }
        }

        public decimal PL
        {
            get => _operation.PL;
            set
            {
                _operation.PL = value;
                CapitalDT = CapitalAT + PL;
                OnPropertyChanged(nameof(PL));
            }
        }

        public decimal CapitalAT
        {
            get => _operation.CapitalAT;
            set
            {
                _operation.CapitalAT = value;
                OnPropertyChanged();
            }
        }

        public decimal CapitalDT
        {
            get => _capitalDT;
            set
            {
                SetProperty(ref _capitalDT, value);
            }
        }
        public float Size
        {
            get => _operation.Size;
            set
            {
                _operation.Size = value;
                OnPropertyChanged();
            }
        }
        public string Note
        {
            get => _operation.Note;
            set
            {
                _operation.Note = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Category> Categories { get; set; } = new ObservableCollection<Category>();
        public ObservableCollection<Tool> Tools { get; } = new ObservableCollection<Tool>();
        public ObservableCollection<Pos> PosList { get; } = new ObservableCollection<Pos>();

        public Category Category
        {
            get => _operation.Category;
            set
            {
                _operation.Category = value;
                OnPropertyChanged();
                RefreshTools();
            }
        }
        public Tool Tool
        {
            get => _operation.Tool;
            set
            {
                _operation.Tool = value;
                OnPropertyChanged();
            }
        }
        public Pos Pos
        {
            get => _operation.Pos;
            set
            {
                _operation.Pos = value;
                OnPropertyChanged();
            }
        }

        public string CategoryText
        {
            get => _categoryText;
            set
            {
                SetProperty(ref _categoryText, value);
            }
        }
        public string ToolText
        {
            get => _toolText;
            set
            {
                SetProperty(ref _toolText, value);
            }
        }


        public ICommand AddCategory => _addCategory ?? (_addCategory = new RelayCommand(AddCategoryInvoked)); 
        public ICommand AddTool => _addTool ?? (_addTool = new RelayCommand(AddToolInvoked));
        public ICommand AddOperation => _addOperation ?? (_addOperation = new RelayCommand(AddOperationInvoked));

        public void OnNavigatedFrom()
        {
        }

        public async void OnNavigatedTo(object parameter)
        {
            Capital capital = await _capitalService.GetLastCapital();

            CapitalAT = capital.Amount;

            await RefreshCategories();

            await RefreshPos();
        }

        private async Task RefreshCategories()
        {
            Categories.Clear();

            CategoryText = string.Empty;

            var cats = await _categoryService.GetCategories();

            foreach (var c in cats)
            {
                Categories.Add(c);
            }
        }

        private void RefreshTools()
        {
            Tools.Clear();

            if (Category is null) return;

            ToolText = string.Empty;

            foreach (var tool in Category.Tools)
            {
                Tools.Add(tool);
            }
        }

        private async Task RefreshPos()
        {
            PosList.Clear();

            var pos = await _posService.GetAllPos();

            foreach (var p in pos)
            {
                PosList.Add(p);
            }
        }

        private async void AddCategoryInvoked()
        {
            if(Category is null && !string.IsNullOrEmpty(CategoryText))
                Category = await _categoryService.AddCategory(CategoryText);

            await RefreshCategories();
        }
        private async void AddToolInvoked()
        {
            if(Tool is null && !string.IsNullOrEmpty(ToolText))
                Tool = await _categoryService.AddTool(Category.Id, ToolText);

            RefreshTools();
        }

        private async void AddOperationInvoked()
        {
            await _operationService.AddGridDataAsync(_operation);
            _navigationService.GoBack();
        }

    }
}
