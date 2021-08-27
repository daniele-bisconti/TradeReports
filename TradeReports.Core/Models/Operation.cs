using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeReports.Core.Models
{
    [Table("Operation")]
    public class Operation
    {
        public Operation(DateTime openDate, DateTime closeDate, int tradeNumber, int monthTradeNumber, decimal capitalAT, decimal pL, Pos pos, float size, Category category, Tool tool, string note = null)
        {
            OpenDate = openDate;
            CloseDate = closeDate;
            TradeNumber = tradeNumber;
            MonthTradeNumber = monthTradeNumber;
            CapitalAT = capitalAT;
            PL = pL;
            Pos = pos;
            Size = size;
            Category = category;
            Tool = tool;
            Note = note;

            Id = new Guid();
            Day = CloseDate.DayOfWeek.ToString().Substring(0, 3);
            GapMinutes = (closeDate - openDate).TotalMinutes;
            CapitalDT = CapitalAT + PL;
        }

        public Operation()
        {

        }

        public Guid Id { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime CloseDate { get; set; }
        public string Day { get; set; }
        public double GapMinutes { get; set; }
        public int TradeNumber { get; set; }
        public int MonthTradeNumber { get; set; }
        public decimal CapitalAT { get; set; }
        public decimal CapitalDT { get; set; }
        public decimal PL { get; set; }
        public virtual Pos Pos { get; set; }
        public float Size { get; set; }
        public virtual Category Category { get; set; }
        public virtual Tool Tool { get; set; }
        public string Note { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Operation operation &&
                   Id.Equals(operation.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
