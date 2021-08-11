using System.Windows.Controls;

using TradeReports.UI.ViewModels;

namespace TradeReports.UI.Views
{
    public partial class ReportsPage : Page
    {
        public ReportsPage(ReportsViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
