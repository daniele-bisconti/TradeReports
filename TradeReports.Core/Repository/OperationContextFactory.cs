using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TradeReports.Core.Repository
{
    public class OperationDbContextFactory : IDesignTimeDbContextFactory<OperationContext>
    {
        public OperationContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OperationContext>();
            optionsBuilder.UseSqlite(@"Data Source=C:\Users\bisco\OneDrive\Documenti\BSCCoding\TradeReports\TradeReports.UI\bin\Release\netcoreapp3.1\publish\OperationsDB.db;");

            return new OperationContext(optionsBuilder.Options);
        }
    }
}
