using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementApi.Data;
using TaskManagementApi.Models;

namespace TaskManagementApi.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly TaskManagementContext _context;

        public UserRepository(TaskManagementContext context)
        {
            _context = context;
        }

        public async System.Threading.Tasks.Task Add(User entity)
        {
            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<User?> FindByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
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
                                 .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async System.Threading.Tasks.Task Update(User entity)
        {
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}