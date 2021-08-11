using System.Collections.Generic;
using System.Threading.Tasks;

using TradeReports.UI.Core.Models;

namespace TradeReports.UI.Core.Contracts.Services
{
    public interface ISampleDataService
    {
        Task<IEnumerable<SampleOrder>> GetGridDataAsync();
    }
}
