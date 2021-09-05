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

        public Category DeepClone()
        {
            return new Category
            {
                Id = Id,
                Description = Description,
                Tools = Tools.Select(t => t.Clone())
            };
        }

        public override bool Equals(object obj)
        {
            return obj is Category category &&
                   Id == category.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
