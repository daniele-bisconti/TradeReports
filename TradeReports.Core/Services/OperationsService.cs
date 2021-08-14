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

        public async Task<bool> AddGridDataAsync(OperationParams operationParams)
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

        public async Task DeleteOperationAsync(string id)
        {
            Operation operation = await _context.Operations.FindAsync(Guid.Parse(id));

            if (operation != null)
            {
                _context.Operations.Remove(operation);

                // Update TradeNumber of other operations
                List<Operation> operations = _context.Operations
                    .Where(o => o.TradeNumber > operation.TradeNumber).ToList();

                operations.ForEach(o => o.TradeNumber--);

                // Update MonthTradeNumber of other operation
                operations = _context.Operations
                    .Where(o => o.CloseDate.Month == operation.CloseDate.Month && o.MonthTradeNumber > operation.MonthTradeNumber)
                    .ToList();

                operations.ForEach(o => o.MonthTradeNumber--);

                await _context.SaveChangesAsync();
            }
            else
                throw new ArgumentException($"No operation with id: {id} ");
        }

        public async Task<IEnumerable<Operation>> GetGridDataAsync()
        {
            List<Operation> data = await _context.Operations.ToListAsync();

            return data;
        }
    }
}
