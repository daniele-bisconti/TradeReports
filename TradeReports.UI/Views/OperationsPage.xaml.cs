using System.Windows.Controls;

using TradeReports.UI.ViewModels;

namespace TradeReports.UI.Views
{
    public partial class OperationsPage : Page
    {
        public OperationsPage(OperationsViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
