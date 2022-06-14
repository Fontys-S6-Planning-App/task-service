namespace task_service.Models;

public class TaskModel
{
    public int Id { get; set; }
    public int ListId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public bool IsValid()
    {
        // check if listId, Name, and Description are not null
        if (ListId < 1 || Name == "" || Description == "")
        {
            return false;
        }
        return true;
    }
}