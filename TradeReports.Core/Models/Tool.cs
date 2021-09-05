using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeReports.Core.Models
{
    [Table("Tool")]
    public class Tool
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public Tool Clone()
        {
            return new Tool { Id = Id, Description = Description };
        }

        public override bool Equals(object obj)
        {
            return obj is Tool tool &&
                   Id == tool.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
