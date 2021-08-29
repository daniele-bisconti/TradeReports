using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeReports.Core.Analitycs.Capital;
using TradeReports.Core.Types;

namespace TradeReports.UI.ViewModels
{
    public class CapitalVariationChartViewModel : ObservableObject
    {
        private CapitalAnalysis _capitalAnalysis;

        public CapitalAnalysis CapitalAnalysis
        {
            get { return _capitalAnalysis; }
            set
            {
                SetCapitalAnalysis(value);
                SetProperty(ref _capitalAnalysis, value);
            }
        }

        public SeriesCollection Series { get; private set; }
        public List<string> Labels { get; private set; }

        public CapitalVariationChartViewModel(CapitalAnalysis capitalAnalysis)
        {
            CapitalAnalysis = capitalAnalysis;
        }

        private void SetCapitalAnalysis(CapitalAnalysis capital)
        {
            Series = new SeriesCollection();
            Labels = new List<string>();

            // Serie visualizzate sul grafico
            Series.Add(new LineSeries { Values = new ChartValues<decimal>(), Name = "Media_Mobile", Title = "Media Mobile (14)", StrokeThickness = 5 });
            Series.Add(new CandleSeries { Values = new ChartValues<OhlcPoint>(), Name = "Variazione_Capitale", Title = "Variazione"});

            // Calcola i valori dei grafici
            var operations = capital.GroupCapitals(DateTimeAggregation.Day);
            var movingAverage = capital.MovingAverage(14);

            // Aggiunta dei punti al grafivo a calndela della variazione del capitale
            foreach (var op in operations)
            {
                Series[1].Values.Add(new OhlcPoint((double)op.Value[0], (double)op.Value.Max(), (double)op.Value.Min(), (double)op.Value[op.Value.Count - 1]));
                Labels.Add(op.Key.ToString("dd/MM/yy"));
            }

            // Aggiunta dei punti al grafico della media mobile
            foreach (var ma in movingAverage)
            {
                Series[0].Values.Add(ma.Value);
                Labels.Add(ma.Key.ToString("dd/MM/yy"));
            }

        }

        private void AddMobileAvg(int period)
        {
            // Serie visualizzate sul grafico
            Series.Add(new LineSeries { Values = new ChartValues<decimal>(), Name = "Media_Mobile", Title = "Media Mobile (14)", StrokeThickness = 5 });


            var movingAverage = CapitalAnalysis.MovingAverage(period);

            // Aggiunta dei punti al grafico della media mobile
            foreach (var ma in movingAverage)
            {
                Series[^1].Values.Add(ma.Value);
                Labels.Add(ma.Key.ToString("dd/MM/yy"));
            }
        }
    }
}
