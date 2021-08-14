using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TradeReports.UI.ViewModels;

namespace TradeReports.UI.Views
{
    /// <summary>
    /// Interaction logic for AddOperationPage.xaml
    /// </summary>
    public partial class AddOperationPage : Page
    {
        public AddOperationPage(AddOperationViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

    }
}
