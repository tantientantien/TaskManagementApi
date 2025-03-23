//using Microsoft.EntityFrameworkCore;

//namespace TaskManagementApi.Repositories
//{
//    public class GenericRepository<T> : IGenericRepository<T> where T : class
//    {
//        private readonly DbContext _context;
//        private readonly DbSet<T> _dbSet;

//        public GenericRepository(DbContext context)
//        {
//            _context = context;
//            _dbSet = context.Set<T>();
//        }

//        public async Task<IEnumerable<T>> GetAll(Func<IQueryable<T>, IQueryable<T>>? query = null)
//        {
//            IQueryable<T> queryable = _dbSet.AsNoTracking();

//            if (query != null)
//            {
//                queryable = query(queryable);
//            }

//            return await queryable.ToListAsync();
//        }

//        public async Task<T> GetById(int id)
//        {
//            var entity = await _dbSet.FindAsync(id);
//            if (entity == null)
//            {
//                throw new KeyNotFoundException($"Entity with ID {id} not found.");
//            }
//            return entity;
//        }

//        public async Task Add(T entity)
//        {
//            await _dbSet.AddAsync(entity);
//            await _context.SaveChangesAsync();
//        }

//        public async Task Update(T entity)
//        {
//            _dbSet.Update(entity);
//            await _context.SaveChangesAsync();
//        }

//        public async Task Delete(int id)
//        {
//            var entity = await GetById(id);
//            _dbSet.Remove(entity);
//            await _context.SaveChangesAsync();
//        }

//        public Task<IEnumerable<T>> GetAll()
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
