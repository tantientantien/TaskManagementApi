using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Data;
using TaskManagementApi.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskManagementApi.Repository
{
    public class LabelRepository : IGenericRepository<Label>
    {
        private readonly TaskManagementContext _context;

        public LabelRepository(TaskManagementContext context)
        {
            _context = context;
        }

        public async Task Add(Label entity)
        {
            await _context.Labels.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var label = await _context.Labels.FindAsync(id);
            if (label != null)
            {
                _context.Labels.Remove(label);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Label>> GetAll()
        {
            return await _context.Labels.ToListAsync();
        }

        public async Task<Label> GetById(int id)
        {
            return await _context.Labels.FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task Update(Label entity)
        {
            _context.Labels.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}