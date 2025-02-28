using TaskManagementApi.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskManagementApi.Interfaces
{
    public interface ITaskLabelRepository : IGenericRepository<TaskLabel>
    {
        Task<TaskLabel> GetTaskLabelById(int taskId, int labelId);
        Task DeleteTaskLabel(int taskId, int labelId);
    }
}
