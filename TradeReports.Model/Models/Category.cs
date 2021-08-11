using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeReports.Core.Models
{
    [Table("Category")]
    public class Category
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public virtual IEnumerable<Tool> Tools { get; set; }
    }
}
