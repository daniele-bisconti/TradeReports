using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TradeReports.Core.Analitycs.Capital;
using TradeReports.Core.Models;

namespace TradeReports.UI.Controls
{
    /// <summary>
    /// Interaction logic for CapitalVariationsChart.xaml
    /// </summary>
    public partial class CapitalVariationsChart : UserControl
    {
        public CapitalVariationsChart()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var d = DataContext;
        }
    }
}
