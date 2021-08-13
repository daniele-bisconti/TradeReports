using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeReports.Core.Interfaces;
using TradeReports.Core.Models;
using TradeReports.Core.Repository;

namespace TradeReports.Core.Services
{
    public class CapitalServiceAsync : ICapitalServiceAsync
    {
        private OperationContext _context;

        public CapitalServiceAsync(OperationContext context)
        {
            _context = context;
        }


        public async Task<Capital> GetLastCapital()
        {
            List<Capital> capitals = await _context.Capital.ToListAsync();
            Capital max = capitals.OrderBy(c => c.Date).LastOrDefault();

            return max;
        }
    }
}
