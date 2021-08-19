using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeReports.Core.Models
{
    [Table("Pos")]
    public class Pos
    {
        public int Id { get; set; }
        public PosType Description { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Pos pos &&
                   Id == pos.Id &&
                   Description == pos.Description;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Description);
        }
    }
}
