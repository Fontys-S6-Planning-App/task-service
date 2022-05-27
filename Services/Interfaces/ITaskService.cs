using task_service.Models;

namespace task_service.Services.Interfaces;

public interface ITaskService
{
    List<TaskModel> GetByListId(int listId);
    void Add(TaskModel task);
    void Update(TaskModel task);
    void Delete(int id);
}