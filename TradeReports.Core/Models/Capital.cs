using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeReports.Core.Models
{
    [Table("Capital")]
    public class Capital
    {
        [Key]
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
    }
}
