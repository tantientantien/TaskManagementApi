using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Data;
using TaskManagementApi.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskManagementApi.Repository
{
    public class TaskCommentRepository : IGenericRepository<TaskComment>
    {
        private readonly TaskManagementContext _context;

        public TaskCommentRepository(TaskManagementContext context)
        {
            _context = context;
        }

        public async Task Add(TaskComment entity)
        {
            await _context.TaskComments.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var comment = await _context.TaskComments.FindAsync(id);
            if (comment != null)
            {
                _context.TaskComments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<TaskComment>> GetAll()
        {
            return await _context.TaskComments
                .Include(tc => tc.Task)
                .Include(tc => tc.User)
                .ToListAsync();
        }

        public async Task<TaskComment> GetById(int id)
        {
            return await _context.TaskComments
                .Include(tc => tc.Task)
                .Include(tc => tc.User)
                .FirstOrDefaultAsync(tc => tc.Id == id);
        }

        public async Task Update(TaskComment entity)
        {
            _context.TaskComments.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}