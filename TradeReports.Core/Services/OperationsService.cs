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

        
        public async Task<bool> AddOperationAsync(OperationParams operationParams)
        {
            Operation operation = CreateOperation(operationParams);

            // Calcola il tradenumber e il month trade number
            SetTradeNumber(operation);
            SetMonthTradeNumber(operation);

            var entity = await _context.Operations.AddAsync(operation);

            // Update Tradenumber e MonthTradeNumber e capital delle altre operazioni
            List<Operation> followingOps = _context.Operations
                .Where(o => o.CloseDate > operation.CloseDate)
                .OrderBy(o => o.CloseDate)
                .ToList();

            Operation prev = operation;

            followingOps.ForEach(o =>
            {
                o.TradeNumber++;
                o.CapitalAT = prev.CapitalDT;
                o.CapitalDT = prev.CapitalDT + o.PL;
                prev = o;
            });

            followingOps
                .Where(o => o.CloseDate.Month == operation.CloseDate.Month && o.CloseDate.Year == operation.CloseDate.Year)
                .ToList().ForEach(o => o.MonthTradeNumber++);

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
                _context.Operations
                    .Where(o => o.CloseDate.Month == operation.CloseDate.Month && o.MonthTradeNumber > operation.MonthTradeNumber)
                    .ToList()
                    .ForEach(o => o.MonthTradeNumber--);

                // Update Capital of following operations
                operations.ForEach(o =>
                {
                    o.CapitalAT -= operation.PL;
                    o.CapitalDT -= operation.PL;
                });

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

        #region Private Methods

        private static Operation CreateOperation(OperationParams operationParams)
        {
            return new Operation(
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
        }

        private void SetTradeNumber(Operation operation)
        {
            operation.TradeNumber = _context.Operations.Where(o => o.CloseDate <= operation.CloseDate).Count() + 1;
        }

        private void SetMonthTradeNumber(Operation operation)
        {
            operation.MonthTradeNumber =
                _context.Operations
                .Where(o => o.CloseDate.Month == operation.CloseDate.Month && o.CloseDate.Year == operation.CloseDate.Year)
                .Where(o => o.CloseDate <= operation.CloseDate)
                .Count() + 1;
        }

        #endregion
    }
}
