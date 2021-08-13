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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            KeyConverter kc = new KeyConverter();
            char c = kc.ConvertToString(e.Key).ToCharArray()[0];

            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
            if (char.IsLetter(c) && c != '.' )
            {
                e.Handled = true;
            }

            base.OnPreviewKeyDown(e);
        }
    }
}
