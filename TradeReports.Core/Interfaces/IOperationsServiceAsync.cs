using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeReports.Core.Models;
using TradeReports.Core.Models.Params;

namespace TradeReports.Core.Interfaces
{
    public interface IOperationsServiceAsync
    {
        Task<IEnumerable<Operation>> GetGridDataAsync();

        Task<bool> AddGridDataAsync(AddOperationParams operationParams);
    }
}
