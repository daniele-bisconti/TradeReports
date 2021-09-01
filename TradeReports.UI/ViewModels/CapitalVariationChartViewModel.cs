using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using TradeReports.Core.Analitycs.Capital;
using TradeReports.Core.Types;

namespace TradeReports.UI.ViewModels
{
    public class CapitalVariationChartViewModel : ObservableObject
    {
        private CapitalAnalysis _capitalAnalysis;

        private long _minValue;
        private long _maxValue;
        private long _unit;
        private long _step;

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
        public Func<double, string> Formatter => value => new DateTime((long)value).ToString("dd/MM/yyyy");

        public long MinValue
        {
            get => _minValue;
            set => SetProperty(ref _minValue, value);
        }

        public long MaxValue
        {
            get => _maxValue;
            set => SetProperty(ref _maxValue, value);
        }


        public long Unit 
        { 
            get => _unit; 
            set => SetProperty(ref _unit, value); 
        }

        public long Step 
        { 
            get => _step; 
            set => SetProperty(ref _step, value); 
        }

        public CapitalVariationChartViewModel(CapitalAnalysis capitalAnalysis)
        {
            CapitalAnalysis = capitalAnalysis;
        }

        private void SetCapitalAnalysis(CapitalAnalysis capital)
        {
            Series = new SeriesCollection();

            AddMovingAverageSeries(capital);
            AddCapitalVariationSeries(capital);

            Unit = TimeSpan.TicksPerDay;
            Step = TimeSpan.TicksPerDay;
            
            SetMinAndMaxChatValues();
        }

        private void SetMinAndMaxChatValues()
        {
            IEnumerable<double> xValues = Series.SelectMany(s => s.Values.GetPoints(s).Select(p => p.X));
            MinValue = (long)xValues.Min();
            MaxValue = (long)xValues.Max() + TimeSpan.FromDays(1).Ticks;
        }

        private void AddCapitalVariationSeries(CapitalAnalysis capital)
        {

            // Serie visualizzate sul grafico
            Series.Add(new CandleSeries(OhlcDateTimePoint.Settings) { Values = new ChartValues<OhlcDateTimePoint>(), Name = "Variazione_Capitale", Title = "Variazione" });

            // Calcola i valori dei grafici
            var operations = capital.GroupCapitals(DateTimeAggregation.Day);

            // Aggiunta dei punti al grafico a calndela della variazione del capitale
            foreach (var op in operations)
            {
                Series[^1].Values.Add(new OhlcDateTimePoint(op.Key, (double)op.Value[0], (double)op.Value.Max(), (double)op.Value.Min(), (double)op.Value[op.Value.Count - 1]));
            }
        }

        private void AddMovingAverageSeries(CapitalAnalysis capital)
        {
            var movingAverage = capital.MovingAverage(14);
            Series.Add(new LineSeries { Values = new ChartValues<DateTimePoint>(), Name = "Media_Mobile", Title = "Media Mobile (14)", StrokeThickness = 5 });

            // Aggiunta dei punti al grafico della media mobile            
            foreach (var ma in movingAverage)
            {
                Series[^1].Values.Add(new DateTimePoint(ma.Key.Date, (double)ma.Value));

            }
        }
    }

    public class OhlcDateTimePoint : OhlcPoint
    {
        public DateTime DateTime { get; set; }

        public OhlcDateTimePoint(DateTime dateTime, double open, double high, double low, double close)
            : base(open, high, low, close)
        {
            DateTime = dateTime;
        }

        public OhlcDateTimePoint()
        {
        }

        public static FinancialMapper<OhlcDateTimePoint> Settings
        {
            get => Mappers.Financial<OhlcDateTimePoint>()
                .X(value => value.DateTime.Ticks)
                .Close(p => p.Close)
                .Open(p => p.Open)
                .High(p => p.High)
                .Low(p => p.Low);
        }
    }
}
