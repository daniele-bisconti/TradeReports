using System.Windows.Controls;

using TradeReports.UI.ViewModels;

namespace TradeReports.UI.Views
{
    public partial class MainPage : Page
    {
        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
