using TaskManagementApi.Models;

namespace TaskManagementApi.Services
{
    public interface ITaskService
    {
        List<TaskItem> GetAllTasks();
        TaskItem GetTaskById(int id);
        void AddTask(TaskItem task);
        void UpdateTask(TaskItem task);
        void DeleteTask(int id);
    }
}
