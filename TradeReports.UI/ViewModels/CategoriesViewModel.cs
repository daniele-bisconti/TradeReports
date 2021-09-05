using MahApps.Metro.Controls.Dialogs;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TradeReports.Core.Interfaces;
using TradeReports.Core.Models;
using TradeReports.UI.Contracts.ViewModels;

namespace TradeReports.UI.ViewModels
{
    public class CategoriesViewModel : ObservableObject, INavigationAware
    {
        private ICategoryServiceAsync _categoryService;

        private RelayCommand _deleteCategory;
        private RelayCommand _deleteTool;
        
        private readonly IDialogCoordinator _dialogCoordinator;


        public ICommand DeleteCategory => _deleteCategory ?? (_deleteCategory = new RelayCommand(OnDeleteCategoryInvoked));
        public ICommand DeleteTool => _deleteTool ?? (_deleteTool = new RelayCommand(OnDeleteToolInvoked));


        public ObservableCollection<Category> Categories { get; set; } = new  ObservableCollection<Category>();
        public ObservableCollection<Tool> Tools { get; set; } = new ObservableCollection<Tool>();

        private Category _category;

        public Category Category
        {
            get { return _category; }
            set
            {
                Category hold = _category;
                SetTool(value);
                bool changed = SetProperty(ref _category, value);

                if (changed)
                    _categoryService.UpdateCategory(hold);
            }
        }

        private Tool _tool;

        public Tool Tool
        {
            get { return _tool; }
            set
            {
                Tool hold = _tool;
                bool changed =  SetProperty(ref _tool, value);

                if(changed)
                    _categoryService.UpdateTool(hold);
            }
        }


        public CategoriesViewModel(ICategoryServiceAsync categoryService)
        {
            _categoryService = categoryService;
            _dialogCoordinator = DialogCoordinator.Instance;
        }

        public void OnNavigatedFrom()
        {
            _categoryService.UpdateTools(Tools);
            _categoryService.UpdateCategory(Category);
        }

        public async void OnNavigatedTo(object parameter)
        {
            await GetCategoriesList();
        }

        private async Task GetCategoriesList()
        {
            List<Category> categories = (await _categoryService.GetCategories()).ToList();
            Categories.Clear();
            
            foreach(Category category in categories)
            {
                Categories.Add(category);
            }
            Tools.Clear();
        }

        private async void OnDeleteCategoryInvoked()
        {
            Category selected = Category;

            try
            {
                MessageDialogResult result = await _dialogCoordinator.ShowMessageAsync(this, "Conferma eliminazione", $"Eliminare la categoria: {selected.Description}", MessageDialogStyle.AffirmativeAndNegative);

                if (result == MessageDialogResult.Affirmative)
                {
                    await _categoryService.DeleteCategory(selected.Id);

                    await GetCategoriesList();
                    Log.Logger.Information($"Removed category with id {selected.Id}");
                }
            }
            catch (OperationCanceledException e)
            {
                Log.Logger.Error(e.ToString());
                await _dialogCoordinator.ShowMessageAsync(this, "Errore", "Impossibile eliminare la categoria. \nLa categoria selezionata potrebbe essere utilizzata da alcune operazioni");
            }
            catch (Exception e)
            {
                Log.Logger.Error(e.ToString());
                throw;
            }
        }

        private async void OnDeleteToolInvoked()
        {
            Tool selected = Tool;

            try
            {
                MessageDialogResult result = await _dialogCoordinator.ShowMessageAsync(this, "Conferma eliminazione", $"Eliminare il Tool: {selected.Description}", MessageDialogStyle.AffirmativeAndNegative);

                if (result == MessageDialogResult.Affirmative)
                {
                    await _categoryService.DeleteTool(selected.Id);

                    SetTool(Category);

                    await GetCategoriesList();
                    Log.Logger.Information($"Removed Tool with id {selected.Id}");
                }
            }
            catch (OperationCanceledException e)
            {
                Log.Logger.Error(e.ToString());
                await _dialogCoordinator.ShowMessageAsync(this, "Errore", "Impossibile eliminare il Tool. \nIl Tool selezionato potrebbe essere utilizzato da alcune operazioni");
            }
            catch (Exception e)
            {
                Log.Logger.Error(e.ToString());
                throw;
            }
        }

        private void SetTool(Category value)
        {
            if (value == null || value.Tools == null) return;
            Tools.Clear();
            foreach (Tool tool in value.Tools)
            {
                Tools.Add(tool);
            }
        }

    }
}
