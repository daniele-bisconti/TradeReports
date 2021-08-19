using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        #region Private Fields
        private OperationParams _operation = new OperationParams();
        private decimal? _capitalDT;

        private string _categoryText;
        private string _toolText;

        private RelayCommand _addCategory;
        private RelayCommand _addTool;
        private RelayCommand _addOperation;
        private DateTime? _openDate;
        private DateTime? _closeDate;
        private decimal? _pl;
        private decimal? _capitalAT;
        private float? _size;
        private string _note;
        private Category _category;
        private Tool _tool;
        private PosType _pos;

        private readonly ICapitalService _capitalService;
        private readonly ICategoryServiceAsync _categoryService;
        private readonly IPosServiceAsync _posService;
        private readonly IOperationsServiceAsync _operationService;
        private readonly INavigationService _navigationService;
        #endregion

        #region Constructors
        public AddOperationViewModel(ICapitalService capitalService, 
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
        #endregion

        #region Property Binding
        [Required]
        public DateTime? OpenDate
        {
            get => _openDate;
            set
            {
                _openDate = value;
                OnPropertyChanged(nameof(OpenDate));
                (AddOperation as RelayCommand).NotifyCanExecuteChanged();
            }
        }

        public DateTime? CloseDate
        {
            get => _closeDate;
            set
            {
                _closeDate = value;
                CapitalAT = value.HasValue ? _capitalService.GetCapitalByDate(value.Value) : _capitalService.GetLastCapital();
                OnPropertyChanged(nameof(CloseDate));
                (AddOperation as RelayCommand).NotifyCanExecuteChanged();
            }
        }

        public decimal? PL
        {
            get => _pl;
            set
            {
                SetProperty(ref _pl, value);
                CapitalDT = CapitalAT + PL;
                (AddOperation as RelayCommand).NotifyCanExecuteChanged();
            }
        }

        public decimal? CapitalAT
        {
            get => _capitalAT;
            set
            {
                _capitalAT = value;
                OnPropertyChanged();
            }
        }

        public decimal? CapitalDT
        {
            get => _capitalDT;
            set
            {
                SetProperty(ref _capitalDT, value);
            }
        }
        public float? Size
        {
            get => _size;
            set
            {
                _size = value;
                OnPropertyChanged();
                (AddOperation as RelayCommand).NotifyCanExecuteChanged();
            }
        }
        public string Note
        {
            get => _note;
            set
            {
                _note = value;
                OnPropertyChanged();
            }
        }

        public Category Category
        {
            get => _category;
            set
            {
                _category = value;
                OnPropertyChanged();
                RefreshTools();
                (AddOperation as RelayCommand).NotifyCanExecuteChanged();
            }
        }

        public Tool Tool
        {
            get => _tool;
            set
            {
                _tool = value;
                OnPropertyChanged();
                (AddOperation as RelayCommand).NotifyCanExecuteChanged();
            }
        }
        public PosType Pos
        {
            get => _pos;
            set
            {
                _pos = value;
                OnPropertyChanged();
                (AddOperation as RelayCommand).NotifyCanExecuteChanged();
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

        #endregion

        #region Observable Lists

        public ObservableCollection<Category> Categories { get; set; } = new ObservableCollection<Category>();
        public ObservableCollection<Tool> Tools { get; } = new ObservableCollection<Tool>();
        public ObservableCollection<Pos> PosList { get; } = new ObservableCollection<Pos>();

        #endregion

        #region Commands

        public ICommand AddCategory => _addCategory ?? (_addCategory = new RelayCommand(AddCategoryInvoked)); 
        public ICommand AddTool => _addTool ?? (_addTool = new RelayCommand(AddToolInvoked));
        public ICommand AddOperation => _addOperation ?? (_addOperation = new RelayCommand(AddOperationInvoked, CanAddOperation));

        private async void AddCategoryInvoked()
        {
            if(Category is null && !string.IsNullOrEmpty(CategoryText))
                Category = await _categoryService.AddCategory(CategoryText);

            await RefreshCategories();
        }
        private async void AddToolInvoked()
        {
            if(Category != null && Tool is null && !string.IsNullOrEmpty(ToolText))
                Tool = await _categoryService.AddTool(Category.Id, ToolText);

            RefreshTools();
        }

        private async void AddOperationInvoked()
        {
            Pos pos = PosList.FirstOrDefault(p => p.Id == (int)_pos);

            _operation = new OperationParams { 
                OpenDate = OpenDate.Value,
                CloseDate = CloseDate.Value,
                CapitalAT = CapitalAT.Value,
                PL = PL.Value,
                Pos = pos,
                Category = Category,
                Tool = Tool,
                Note = Note,
                Size = Size.Value
            };

            await _operationService.AddOperationAsync(_operation);
            _navigationService.GoBack();
        }

        private bool CanAddOperation()
        {
            return Category != null
                && Tool != null
                && OpenDate.HasValue
                && CloseDate.HasValue
                && PL.HasValue
                && Size.HasValue;
        }

        #endregion

        #region Private Methods

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

            ToolText = string.Empty;

            if (Category is null) return;

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

        #endregion

        #region INavigationAware Implementation

        public void OnNavigatedFrom()
        {
        }

        public async void OnNavigatedTo(object parameter)
        {
            decimal capital = _capitalService.GetLastCapital();

            CapitalAT = capital;

            Pos = PosType.Long;

            await RefreshCategories();

            await RefreshPos();
        }

        #endregion
    }
}
