using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeReports.Core.Types;

namespace TradeReports.Core.Interfaces
{
    public interface IOperationsAnalysisService
    {
        Task<Dictionary<DateTime, decimal>> GetCapitalVariation(DateTimeAggregation aggregation);
    }
}
