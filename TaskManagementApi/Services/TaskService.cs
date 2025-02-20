using TaskManagementApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;


namespace TaskManagementApi.Services
{
    public class TaskService : ITaskService
    {
        private readonly List<TaskItem> tasks;

        public TaskService()
        {
            tasks = new List<TaskItem>();
        }

        public void AddTask(TaskItem task)
        {
            task.Id = tasks.Any() ? tasks.Max(t => t.Id) + 1 : 1;
            tasks.Add(task);
        }

        public void DeleteTask(int id)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                tasks.Remove(task);
            }
            else
            {
                throw new KeyNotFoundException($"Task with ID {id} not found.");
            }
        }

        public List<TaskItem> GetAllTasks()
        {
            return tasks;
        }

        public TaskItem GetTaskById(int id)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                throw new KeyNotFoundException($"Task with ID {id} not found.");
            }
            return task;
        }

        public void UpdateTask(TaskItem updatedTask)
        {
            var existingTask = tasks.FirstOrDefault(t => t.Id == updatedTask.Id);
            if (existingTask == null)
            {
                throw new KeyNotFoundException($"Task with ID {updatedTask.Id} not found.");
            }

            existingTask.Title = updatedTask.Title;
            existingTask.Completed = updatedTask.Completed;
            existingTask.AssignedTo = updatedTask.AssignedTo;
            existingTask.DueDate = updatedTask.DueDate;
        }
    }
}