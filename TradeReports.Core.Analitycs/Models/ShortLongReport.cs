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
        public int ShortPercentage { get => NumOfShort == 0? 0 : (int)Math.Round((double)(NumOfShort * 100) / TotalTrades); }
        public int LongPercentage { get => NumOfLong == 0 ? 0 : (int)Math.Round((double)(NumOfLong * 100) / TotalTrades); }

        public int NumOfLongProfit
        {
            get => _operations.Count(op => op.Pos.Description == PosType.Long && op.PL > 0 && op.Category.Description.ToUpper().Trim() != "MARKET CAP");
        }
        public int NumOfLongLoss
        {
            get => _operations.Count(op => op.Pos.Description == PosType.Long && op.PL < 0 && op.Category.Description.ToUpper().Trim() != "MARKET CAP");
        }
        public decimal AmountOfLongProfit
        {
            get => _operations
                .Where(op => op.Pos.Description == PosType.Long && op.PL > 0 && op.Category.Description.ToUpper().Trim() != "MARKET CAP")
                .Sum(op => op.PL);
        }
        public decimal AmountOfLongLoss
        {
            get => Math.Abs(_operations
                .Where(op => op.Pos.Description == PosType.Long && op.PL < 0 && op.Category.Description.ToUpper().Trim() != "MARKET CAP")
                .Sum(op => op.PL));
        }
        public int LongProfitPercentage
        {
            get => NumOfLongProfit == 0 ? 0 : (int)Math.Round((double)(NumOfLongProfit * 100) / NumOfLong); 
        }
        public int LongLossPercentage
        {
            get => NumOfLongLoss == 0 ? 0 : (int)Math.Round((double)(NumOfLongLoss * 100) / NumOfLong);
        }

        public int NumOfShortProfit
        {
            get => _operations.Count(op => op.Pos.Description == PosType.Short && op.PL > 0 && op.Category.Description.ToUpper().Trim() != "MARKET CAP");
        }
        public int NumOfShortLoss
        {
            get => _operations.Count(op => op.Pos.Description == PosType.Short && op.PL < 0 && op.Category.Description.ToUpper().Trim() != "MARKET CAP");
        }
        public decimal AmountOfShortLoss
        {
            get =>  Math.Abs( _operations
                .Where(op => op.Pos.Description == PosType.Short && op.PL < 0 && op.Category.Description.ToUpper().Trim() != "MARKET CAP")
                .Sum(op => op.PL));
        }
        public decimal AmountOfShortProfit
        {
            get => _operations
                .Where(op => op.Pos.Description == PosType.Short && op.PL > 0 && op.Category.Description.ToUpper().Trim() != "MARKET CAP")
                .Sum(op => op.PL);
        }
        public int ShortProfitPercentage
        {
            get => NumOfShortProfit == 0 ? 0 : (int)Math.Round((double)(NumOfShortProfit * 100) / NumOfShort);
        }
        public int ShortLossPercentage
        {
            get => NumOfShortLoss == 0 ? 0 : (int)Math.Round((double)(NumOfShortLoss * 100) / NumOfShort);
        }

        private IEnumerable<Operation> _operations;

        public ShortLongReport(IEnumerable<Operation> operations)
        {
            _operations = operations;

            TotalTrades = operations.Count( op => op.Category.Description.ToUpper().Trim() != "MARKET CAP");
            NumOfShort = operations.Count(op => op.Pos.Description == PosType.Short && op.Category.Description.ToUpper().Trim() != "MARKET CAP");
            NumOfLong = operations.Count(op => op.Pos.Description == PosType.Long && op.Category.Description.ToUpper().Trim() != "MARKET CAP");
        }


    }
}
