using TaskManagementApi.Dtos.TaskLabel;
using TaskManagementApi.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskManagementApi.Interfaces
{
    public interface ITaskLabelRepository : IGenericRepository<TaskLabel>
    {
        Task<List<TaskLabelDataDto>> GetTaskLabelById(int taskId);

        Task<TaskLabel> GetTaskLabelById(int taskId, int labelId);

        Task DeleteTaskLabel(int taskId, int labelId);
    }
}
