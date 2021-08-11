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
    public class PosServiceAsync : IPosServiceAsync
    {
        private OperationContext _context;

        public PosServiceAsync(OperationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Pos>> GetAllPos()
        {
            return await _context.Pos.ToListAsync();
        }

        public async Task<Pos> GetPosById(int id)
        {
            Pos pos = await _context.Pos.FindAsync(id);

            return pos;
        }
    }
}
