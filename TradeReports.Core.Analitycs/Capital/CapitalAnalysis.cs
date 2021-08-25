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
            var variation = GroupCapitals(aggregation)
                .Select(op => new KeyValuePair<DateTime, decimal>(op.Key, op.Value.Last()));

            Dictionary<DateTime, decimal> result = new(variation);

            return result;
        }

        public Dictionary<DateTime, List<decimal>> GroupCapitals(DateTimeAggregation aggregation)
        {
            return Operations
                            .ToLookup(op =>
                            {

                                switch (aggregation)
                                {
                                    case DateTimeAggregation.Day:
                                        return op.CloseDate.Date;
                                }

                                return op.CloseDate.Date;
                            })
                            .ToDictionary(o => o.Key, o => o.OrderBy(o => o.CloseDate).Select(o => o.CapitalDT).ToList());
        }

        public Dictionary<DateTime, decimal> MovingAverage(int period)
        {
            Dictionary<DateTime, decimal> capitals = this.CapitalVariation(DateTimeAggregation.Day);
            Dictionary<DateTime, decimal> movingAvgs = new();

            for(int i = 0; i < capitals.Count; i++)
            {
                decimal num = 0;
                int den = 0;

                for(int j = 0, z = i + 1 < period ? i + 1 : period; j < period && i - j >= 0; j++, z--)
                {
                    num += capitals[capitals.Keys.ElementAt(i - j)] * (z);
                    den += j + 1;
                }

                movingAvgs[capitals.Keys.ElementAt(i)] = num / den;
            }

            return movingAvgs;
        }
    }
}
