using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeReports.Core.Models;

namespace TradeReports.Core.Repository
{
    public class OperationContext : DbContext
    { 
        public OperationContext(DbContextOptions options) : base(options) { }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tool> Tools { get; set; }
        public DbSet<Pos> Pos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Pos>()
                .Property(e => e.Description)
                .HasConversion(
                    e => e.ToString(),
                    e => (PosType)Enum.Parse(typeof(PosType), e)
                    );
        }
    }
}
