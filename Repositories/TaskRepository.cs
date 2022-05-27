using task_service.DBContexts;
using task_service.Models;
using task_service.Repositories.Interfaces;

namespace task_service.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly TaskContext _context;
    
    public TaskRepository(TaskContext context)
    {
        _context = context;
    }
    
    public List<TaskModel> GetByListId(int listId)
    {
        return _context.Tasks.Where(t => t.ListId == listId).ToList();
    }

    public void Add(TaskModel task)
    {
        _context.Tasks.Add(task);
        _context.SaveChanges();
    }

    public void Update(TaskModel task)
    {
        _context.Tasks.Update(task);
        _context.SaveChanges();
    }

    public void Delete(int taskId)
    {
        var task = _context.Tasks.Find(taskId);
        if (task == null)
        {
            throw new Exception("Task not found");
        }
        _context.Tasks.Remove(task);
        _context.SaveChanges();
    }
}