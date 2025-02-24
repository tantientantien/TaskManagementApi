using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Data;
using TaskManagementApi.Models;

namespace TaskManagementApi.Repository
{
    public class TaskLabelRepository : IGenericRepository<TaskLabel>
    {
        private readonly TaskManagementContext _context;

        public TaskLabelRepository(TaskManagementContext context)
        {
            _context = context;
        }

        public async System.Threading.Tasks.Task Add(TaskLabel entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _context.TaskLabels.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public Task<TaskLabel> GetById(int id)
        {
            throw new NotSupportedException("TaskLabel uses a composite key. Use GetByIdAsync(int taskId, int labelId) instead.");
        }

        public System.Threading.Tasks.Task Delete(int id)
        {
            throw new NotSupportedException("TaskLabel uses a composite key. Use DeleteAsync(int taskId, int labelId) instead.");
        }

        public async Task<IEnumerable<TaskLabel>> GetAll()
        {
            return await _context.TaskLabels
                                 .Include(tl => tl.Task)
                                 .Include(tl => tl.Label)
                                 .ToListAsync();
        }

        public async System.Threading.Tasks.Task Update(TaskLabel entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _context.TaskLabels.Update(entity);
            await _context.SaveChangesAsync();
        }


        public async Task<TaskLabel> GetById(int taskId, int labelId)
        {
            return await _context.TaskLabels.FindAsync(taskId, labelId);
        }


        public async System.Threading.Tasks.Task Delete(int taskId, int labelId)
        {
            var entity = await _context.TaskLabels.FindAsync(taskId, labelId);
            if (entity != null)
            {
                _context.TaskLabels.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}