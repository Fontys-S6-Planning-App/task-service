namespace task_service.Models;

public class TaskModel
{
    public int Id { get; set; }
    public int ListId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}