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
        List<TaskModel> tasks = _taskRepository.GetByListId(listId);
        //if tasks is empty throw not found exception
        if(tasks.Count > 0)
        {
            return tasks;
        }
        throw new Exception("No tasks found for list id: " + listId);
    }

    public void Add(TaskModel task)
    {
        //check if task is valid
        if(task.IsValid())
        {
            _taskRepository.Add(task);
        }
        else
        {
            throw new Exception("Task is not valid");
        }
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