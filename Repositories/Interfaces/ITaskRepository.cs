using task_service.Models;

namespace task_service.Repositories.Interfaces;

public interface ITaskRepository
{
    List<TaskModel> GetByListId(int listId);
    void Add(TaskModel task);
    void Update(TaskModel task);
    void Delete(int taskId);
    void DeleteByListId(int listId);
}