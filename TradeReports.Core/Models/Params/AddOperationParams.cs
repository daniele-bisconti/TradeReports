using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeReports.Core.Models.Params
{
    public class AddOperationParams
    {
        public DateTime OpenDate { get; set; }
        public DateTime CloseDate { get; set; }
        public decimal CapitalAT { get; set; }
        public decimal PL { get; set; }
        public Pos Pos { get; set; }
        public Category Category { get; set; }
        public Tool Tool { get; set; }
        public string Note { get; set; }
        public int TradeNumber { get; set; }
        public int MonthTradeNumber { get; set; }
        public float Size { get; set; }
    }
}
