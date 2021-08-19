using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeReports.Core.Models;

namespace TradeReports.Core.Interfaces
{
    public interface ICapitalService
    {
        decimal GetLastCapital();

        decimal GetCapitalByDate(DateTime date);
    }
}
