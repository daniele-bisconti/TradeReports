using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeReports.Core.Types;

namespace TradeReports.Core.Analytics.Interfaces
{
    public interface IOperationsAnalysisService
    {
        Task<Dictionary<DateTime, decimal>> GetCapitalVariation(DateTimeAggregation aggregation);
        Task<Dictionary<DateTime, List<decimal>>> GetGroupedCapitals(DateTimeAggregation aggregation);
        Task<Dictionary<DateTime, decimal>> GetMovingAverage(int period);
    }
}
