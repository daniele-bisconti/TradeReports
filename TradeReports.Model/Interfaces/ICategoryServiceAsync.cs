using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeReports.Core.Models;

namespace TradeReports.Core.Interfaces
{
    public interface ICategoryServiceAsync
    {
        Task<Category> GetCategoryById(int id);
        Task<IEnumerable<Category>> GetCategories();
        Task<Category> AddCategory(string description);
        Task<Tool> AddTool(int categoryId, string toolDescription);
    }
}
