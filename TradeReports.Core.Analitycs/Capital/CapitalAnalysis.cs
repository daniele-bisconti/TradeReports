using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeReports.Core.Models;
using TradeReports.Core.Types;

namespace TradeReports.Core.Analitycs.Capital
{
    public class CapitalAnalysis
    {
        public IList<Operation> Operations { get; set; }

        public CapitalAnalysis(IList<Operation> operations)
        {
            Operations = operations;
        }

        public CapitalAnalysis()
        {
            Operations = new List<Operation>();
        }

        public Dictionary<DateTime, decimal> CapitalVariation(DateTimeAggregation aggregation)
        {
            var variation = Operations
                .GroupBy(op => {

                    switch (aggregation)
                    {
                        case DateTimeAggregation.Day:
                            return op.CloseDate.Date;
                    }

                    return op.CloseDate.Date;
                })
                .Select(op => new KeyValuePair<DateTime, decimal>(op.Key, op.Sum(op => op.CapitalDT)));

            Dictionary<DateTime, decimal> result = new(variation);

            return result;
        }
    }
}
