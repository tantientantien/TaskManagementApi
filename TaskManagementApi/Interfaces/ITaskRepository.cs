
using Task =  TaskManagementApi.Models.Task;

namespace TaskManagementApi.Interfaces
{
    public interface ITaskRepository: IGenericRepository<Task>
    {
        Task<IEnumerable<Task>> GetAllQueryable(int? category, List<int>? labelIds);
    }
}
