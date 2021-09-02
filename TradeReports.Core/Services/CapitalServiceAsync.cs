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
    public class CapitalServiceAsync : ICapitalService
    {
        private OperationContext _context;

        public CapitalServiceAsync(OperationContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// Restituisce il Capital DT dell'operazione precedente alla data indicata, più vicina
        /// </summary>
        /// <param name="date">Data dalla quale recuperare il capitale</param>
        /// <returns>Capitale</returns>
        public decimal GetCapitalByDate(DateTime date)
        {
            Operation operation = _context.Operations
                .Where(o => o.CloseDate <= date)
                .OrderBy(o => o.TradeNumber)
                .LastOrDefault();

            return operation != null ? operation.CapitalDT : 0;
        }

        public decimal GetLastCapital()
        {
            Operation operation = _context.Operations
                .OrderBy(o => o.TradeNumber)
                .LastOrDefault();

            return operation != null ? operation.CapitalDT : 0;
        }

    }
}
