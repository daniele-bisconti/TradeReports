using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradeReports.Core.Interfaces;
using TradeReports.Core.Models;
using TradeReports.Core.Repository;

namespace TradeReports.Core.Services
{
    public class CategoryServiceAsync : ICategoryServiceAsync
    {
        private OperationContext _context;
        public CategoryServiceAsync(OperationContext context)
        {
            _context = context;
        }

        public async Task<Category> AddCategory(string description)
        {
            Category category = new Category { Description = description };

            var entity = await _context.Categories.AddAsync(category);

            _context.SaveChanges();

            entity.Entity.Tools = new List<Tool>();

            return entity.Entity;
        }

        public async Task<Tool> AddTool(int categoryId, string toolDescription)
        {
            Category category = await  _context.Categories.FindAsync(categoryId);

            if (category is null) return null;

            var entity = await _context.Tools.AddAsync(new Tool { Description = toolDescription });

            await _context.SaveChangesAsync();

            Tool tool = entity.Entity;

            List<Tool> tools = category.Tools.ToList();
            tools.Add(tool);

            category.Tools = tools;


            _context.Categories.Update(category);

            await _context.SaveChangesAsync();

            return tool;
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            IEnumerable<Category> categories = await _context.Categories.ToListAsync();

            return categories;
        }

        public async Task<Category> GetCategoryById(int id)
        {
            Category category = await _context.Categories.FindAsync(id);

            return category;
        }
    }
}
