using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementApi.Data;
using Task = TaskManagementApi.Models.Task;

namespace TaskManagementApi.Repositories
{
    public class TaskRepository : IGenericRepository<Task>
    {
        private readonly TaskManagementContext _context;

        public TaskRepository(TaskManagementContext context)
        {
            _context = context;
        }

        public async System.Threading.Tasks.Task Add(Task entity)
        {
            await _context.Tasks.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task Delete(int id)
        {
            var entity = await _context.Tasks.FindAsync(id);
            if (entity != null)
            {
                _context.Tasks.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Task>> GetAll()
        {
            return await _context.Tasks.ToListAsync();
        }

        public async Task<Task> GetById(int id)
        {
            return await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async System.Threading.Tasks.Task Update(Task entity)
        {
            _context.Tasks.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}