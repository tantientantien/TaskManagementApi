using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Data;
using TaskManagementApi.Interfaces;
using TaskManagementApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task = TaskManagementApi.Models.Task;

namespace TaskManagementApi.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskManagementContext _context;

        public TaskRepository(TaskManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Task>> GetAllQueryable(int? category, List<int>? labelIds)
        {
            var query = _context.Tasks
                .Include(t => t.Attachments)
                .Include(t => t.TaskComments)
                .Include(t => t.TaskLabels)
                    .ThenInclude(tl => tl.Label)
                .Include(t => t.Assignee)
                .AsNoTracking();

            if (category.HasValue)
            {
                query = query.Where(t => t.CategoryId == category.Value);
            }

            if (labelIds is { Count: > 0 })
            {
                query = query.Where(t => t.TaskLabels.Any(tl => labelIds.Contains(tl.LabelId)));
            }

            return await query.ToListAsync();
        }


        public async Task<IEnumerable<Task>> GetAll()
        {
            return await _context.Tasks
                .Include(t => t.Attachments)
                .Include(t => t.TaskComments)
                .Include(t => t.Assignee)
                .AsNoTracking()
                .ToListAsync();
        }


        public async Task<Task> GetById(int id)
        {
            return await _context.Tasks
                .AsNoTracking()
                .Include(t => t.User)
                .Include(t => t.Assignee)
                .Include(t => t.Category)
                .Include(t => t.TaskComments)
                    .ThenInclude(tc => tc.User)
                .Include(t => t.Attachments)
                .Include(t => t.TaskLabels)
                .FirstOrDefaultAsync(t => t.Id == id);
        }


        public async System.Threading.Tasks.Task Add(Task entity)
        {
            await _context.Tasks.AddAsync(entity);
            await _context.SaveChangesAsync();
        }


        public async System.Threading.Tasks.Task Update(Task entity)
        {
            var existingTask = await _context.Tasks.FindAsync(entity.Id);
            if (existingTask == null)
            {
                throw new KeyNotFoundException($"Task with ID {entity.Id} not found.");
            }

            _context.Entry(existingTask).CurrentValues.SetValues(entity);

            existingTask.UserId = entity.UserId;
            existingTask.AssigneeId = entity.AssigneeId;
            existingTask.CategoryId = entity.CategoryId;

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
    }
}
