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

        /// <summary>
        /// Aggiunge una nuova operazione. Se sono presenti altre operazioni 
        /// aggiorna i capitali delle operazioni successive
        /// </summary>
        /// <param name="operationParams">Parametri di creazione della nuova operazione</param>
        /// <returns>True se l'operazione è stata inserita con successo</returns>
        Task<bool> AddOperationAsync(OperationParams operationParams);

        /// <summary>
        /// Elimina una operazione dalla lista delle operaizoni.
        /// Aggiorna il capitale delle operazione successive sottraendo il P&L dell'operazione
        /// eliminata
        /// </summary>
        /// <param name="id">Id dell'operazione da eliminare</param>
        Task DeleteOperationAsync(string id);
    }
}
