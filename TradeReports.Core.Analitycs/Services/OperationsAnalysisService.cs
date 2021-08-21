using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeReports.Core.Analitycs.Capital;
using TradeReports.Core.Interfaces;
using TradeReports.Core.Repository;
using TradeReports.Core.Types;

namespace TradeReports.Core.Services
{
    public class OperationsAnalysisService : IOperationsAnalysisService
    {
        private OperationContext _context;
        public OperationsAnalysisService(OperationContext context)
        {
            _context = context;
        }

        public async Task<Dictionary<DateTime, decimal>> GetCapitalVariation(DateTimeAggregation aggregation)
        {
            var operations = await _context.Operations.ToListAsync();

            CapitalAnalysis capitalAnalysis = new CapitalAnalysis(operations.OrderBy(op => op.CloseDate).ToList());

            return capitalAnalysis.CapitalVariation(aggregation);
        }
    }
}
