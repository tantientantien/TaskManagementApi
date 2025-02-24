using TaskManagementApi.Models;

public interface IUserRepository : IGenericRepository<User>
{ 
    Task<User?> FindByEmail(string email);
}
