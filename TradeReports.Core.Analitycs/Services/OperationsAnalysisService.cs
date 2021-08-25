using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeReports.Core.Analitycs.Capital;
using TradeReports.Core.Analytics.Interfaces;
using TradeReports.Core.Interfaces;
using TradeReports.Core.Repository;
using TradeReports.Core.Types;

namespace TradeReports.Core.Analytics.Services
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

        public async Task<Dictionary<DateTime, List<decimal>>> GetGroupedCapitals(DateTimeAggregation aggregation)
        {
            var operations = await _context.Operations.ToListAsync();

            CapitalAnalysis capitalAnalysis = new CapitalAnalysis(operations.OrderBy(op => op.CloseDate).ToList());
            return capitalAnalysis.GroupCapitals(aggregation);
        }

        public async Task<Dictionary<DateTime, decimal>> GetMovingAverage(int period)
        {
            var operations = await _context.Operations.ToListAsync();

            CapitalAnalysis capitalAnalysis = new CapitalAnalysis(operations.OrderBy(op => op.CloseDate).ToList());
            return capitalAnalysis.MovingAverage(period);
        }
    }
}
