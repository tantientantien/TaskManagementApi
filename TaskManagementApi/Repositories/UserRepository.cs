using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Data;
using TaskManagementApi.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskManagementApi.Repository
{
    public class UserRepository : IGenericRepository<User>
    {
        private readonly TaskManagementContext _context;

        public UserRepository(TaskManagementContext context)
        {
            _context = context;
        }

        public async Task Add(User entity)
        {
            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users
                                 .Include(u => u.Tasks)
                                 .Include(u => u.TaskComments)
                                 .ToListAsync();
        }

        public async Task<User> GetById(int id)
        {
            return await _context.Users
                                 .Include(u => u.Tasks)
                                 .Include(u => u.TaskComments)
                                 .FirstOrDefaultAsync(u => int.Parse(u.Id) == id);
        }

        public async Task Update(User entity)
        {
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}