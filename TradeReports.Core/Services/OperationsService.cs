using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeReports.Core.Models;
using TradeReports.Core.Repository;
using TradeReports.Core.Interfaces;
using TradeReports.Core.Models.Params;

namespace TradeReports.Core.Services
{
    public class OperationsService : IOperationsServiceAsync
    {
        private OperationContext _context;
        private IPosServiceAsync _posService;
        private ICategoryServiceAsync _categoryService;

        public OperationsService(OperationContext context, IPosServiceAsync posServiceAsync, ICategoryServiceAsync categoryServiceAsync)
        {
            _context = context;
            _posService = posServiceAsync;
            _categoryService = categoryServiceAsync;
        }

        public async Task<bool> AddGridDataAsync(AddOperationParams operationParams)
        {
            Operation operation = new Operation(
                operationParams.OpenDate,
                operationParams.CloseDate,
                operationParams.TradeNumber,
                operationParams.MonthTradeNumber,
                operationParams.CapitalAT,
                operationParams.PL,
                operationParams.Pos,
                operationParams.Size,
                operationParams.Category,
                operationParams.Tool,
                operationParams.Note
                );

            operation.TradeNumber = _context.Operations.Count() + 1;
            operation.MonthTradeNumber = _context.Operations
                .Where(o => o.CloseDate.Month == operation.CloseDate.Month && o.CloseDate.Year == operation.CloseDate.Year)
                .Count() + 1;

            var entity = await _context.Operations.AddAsync(operation);

            await _context.SaveChangesAsync();

            return entity.Entity != null;
        }

        public async Task<IEnumerable<Operation>> GetGridDataAsync()
        {
            List<Operation> data = await _context.Operations.ToListAsync();

            return data;
        }
    }
}
