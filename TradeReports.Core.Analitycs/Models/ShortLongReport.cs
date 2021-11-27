using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeReports.Core.Models;

namespace TradeReports.Core.Analitycs.Models
{
    public class ShortLongReport
    {
        public int TotalTrades { get; private set; }
        public int NumOfShort { get; private set; }
        public int NumOfLong { get; private set; }
        public int ShortPercentage { get => (int)Math.Round((double)(NumOfShort * 100) / TotalTrades); }
        public int LongPercentage { get => (int)Math.Round((double)(NumOfLong * 100) / TotalTrades); }

        public int NumOfLongProfit
        {
            get => _operations.Count(op => op.Pos.Description == PosType.Long && op.PL > 0);
        }
        public int NumOfLongLoss
        {
            get => _operations.Count(op => op.Pos.Description == PosType.Long && op.PL < 0);
        }
        public int LongProfitPercentage
        {
            get => (int)Math.Round((double)(NumOfLongProfit * 100) / NumOfLong); 
        }
        public int LongLossPercentage
        {
            get => (int)Math.Round((double)(NumOfLongLoss * 100) / NumOfLong);
        }

        public int NumOfShortProfit
        {
            get => _operations.Count(op => op.Pos.Description == PosType.Short && op.PL > 0);
        }
        public int NumOfShortLoss
        {
            get => _operations.Count(op => op.Pos.Description == PosType.Short && op.PL < 0);
        }
        public int ShortProfitPercentage
        {
            get => (int)Math.Round((double)(NumOfShortProfit * 100) / NumOfShort);
        }
        public int ShortLossPercentage
        {
            get => (int)Math.Round((double)(NumOfShortLoss * 100) / NumOfShort);
        }

        private IEnumerable<Operation> _operations;

        public ShortLongReport(IEnumerable<Operation> operations)
        {
            _operations = operations;

            TotalTrades = operations.Count();
            NumOfShort = operations.Count(op => op.Pos.Description == PosType.Short);
            NumOfLong = TotalTrades - NumOfShort;
        }


    }
}
