using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using task_service.Models;
using task_service.Services.Interfaces;

namespace task_service.Controllers;

[Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("tasks")]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;
    
    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }
    
    [HttpGet("{id}")]
    public IList<TaskModel> GetByListId(int id)
    {
        return _taskService.GetByListId(id);
    }
    
    [HttpPost]
    public void Create([FromBody] TaskModel task)
    {
        _taskService.Add(task);
    }
    
    [HttpPut]
    public void Update([FromBody] TaskModel task)
    {
        _taskService.Update(task);
    }
    
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
        _taskService.Delete(id);
    }
}