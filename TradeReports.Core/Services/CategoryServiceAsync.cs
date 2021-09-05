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

        public async Task<Category> DeleteCategory(int id)
        {
            Category category = await _context.Categories.FindAsync(id);

            var operations = await _context.Operations.AnyAsync(o => o.Category.Equals(category));

            if (operations) throw new OperationCanceledException("Category used by Operations");

            if(category is null) return null;

            // Rimozione dei tool associati alla categoria
            _context.Tools.RemoveRange(category.Tools);

            Category removed = _context.Categories.Remove(category).Entity;

            _context.SaveChanges();

            return removed;
        }

        public Category UpdateCategory(Category category)
        {
            if(category is null || !_context.Categories.Contains(category)) return null;

            var updated = _context.Categories.Update(category);
            _context.SaveChanges();

            return updated.Entity;
        }

        public async Task<Tool> DeleteTool(int id)
        {
            Tool tool = _context.Tools.Find(id);
            if (tool is null) return null;

            var operations = await _context.Operations.AnyAsync(o => o.Tool.Equals(tool));

            if (operations) throw new OperationCanceledException("Tool used by Operations");

            var res = _context.Tools.Remove(tool).Entity;

            _context.SaveChanges();

            return res;
        }

        public Tool UpdateTool(Tool tool)
        {
            if(tool is null || !_context.Tools.Contains(tool)) return null;

            var updated = _context.Tools.Update(tool);
            _context.SaveChanges();

            return updated.Entity;
        }

        public void UpdateCategories(IEnumerable<Category> categories)
        {
            if(categories is null || !categories.Any()) return;

            foreach (Category category in categories)
            {
                var c = _context.Categories.Find(category.Id);
                c.Description = category.Description;
            }

            _context.SaveChanges();
        }

        public void UpdateTools(IEnumerable<Tool> tools)
        {
            if(tools is null || !tools.Any()) return;

            foreach(Tool tool in tools)
            {
                var t = _context.Tools.Find(tool.Id);
                t.Description = tool.Description;
            }

            _context.SaveChanges();
        }
    }
}
