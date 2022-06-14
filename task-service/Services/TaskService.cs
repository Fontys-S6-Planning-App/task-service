using task_service.Models;
using task_service.Repositories.Interfaces;
using task_service.Services.Interfaces;

namespace task_service.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    
    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }
    
    public List<TaskModel> GetByListId(int listId)
    {
        return _taskRepository.GetByListId(listId);
    }

    public void Add(TaskModel task)
    {
        _taskRepository.Add(task);
    }

    public void Update(TaskModel task)
    {
        _taskRepository.Update(task);
    }

    public void Delete(int id)
    {
        _taskRepository.Delete(id);
    }
    
    public void DeleteByListId(int listId)
    {
        _taskRepository.DeleteByListId(listId);
    }
}