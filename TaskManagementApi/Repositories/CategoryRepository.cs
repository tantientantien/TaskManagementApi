using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Data;
using TaskManagementApi.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskManagementApi.Repository
{
    public class CategoryRepository : IGenericRepository<Category>
    {
        private readonly TaskManagementContext _context;

        public CategoryRepository(TaskManagementContext context)
        {
            _context = context;
        }

        public async Task Add(Category entity)
        {
            await _context.Categories.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            return await _context.Categories
                                 .Include(c => c.Tasks)
                                 .ToListAsync();
        }

        public async Task<Category> GetById(int id)
        {
            return await _context.Categories
                                 .Include(c => c.Tasks)
                                 .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task Update(Category entity)
        {
            _context.Categories.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}