using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using task_service;
using Moq;
using task_service.Models;
using task_service.Repositories.Interfaces;
using task_service.Services;

namespace task_service.tests.UnitTests;

public class TaskServiceTests
{
    private readonly Mock<ITaskRepository> _taskRepositoryMock;
    private readonly TaskService _taskService;
    
    public TaskServiceTests()
    {
        _taskRepositoryMock = new Mock<ITaskRepository>();
        _taskService = new TaskService(_taskRepositoryMock.Object);
    }
    
    [Fact]
    public void GetByListId_ReturnsTasks()
    {
        // Arrange
        var listId = 1;
        _taskRepositoryMock.Setup(x => x.GetByListId(listId)).Returns(new List<TaskModel>{new TaskModel() {Id = 1, ListId = 1, Name = "Task 1"}});
        
        // Act
        var result = _taskService.GetByListId(listId);
        
        // Assert
        Assert.Equal(1, result[0].Id);
    }
    
    [Fact]
    public void GetByListId_ReturnsEmptyList()
    {
        // Arrange
        var listId = 1;
        _taskRepositoryMock.Setup(x => x.GetByListId(listId)).Returns(new List<TaskModel>());
        
        // Act and Assert
        Assert.Throws<Exception>(() => _taskService.GetByListId(listId));
    }
    
    [Fact]
    public void Add_AddsTask()
    {
        // Arrange
        var task = new TaskModel {Id = 1, ListId = 1, Name = "Task 1", Description = "Description 1"};
        _taskRepositoryMock.Setup(x => x.Add(task));
        
        // Act
        var exception = Record.Exception(() => _taskService.Add(task));
        
        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Add_ThrowsException()
    {
        // Arrange
        var task = new TaskModel {Id = 1, ListId = 1, Name = "Task 1", Description = ""};
        _taskRepositoryMock.Setup(x => x.Add(task));

        // Act and Assert
        Assert.Throws<Exception>(() => _taskService.Add(task));
    }
    
    [Fact]
    public void Update_UpdatesTask()
    {
        // Arrange
        var task = new TaskModel {Id = 1, ListId = 1, Name = "Task 1", Description = "Description 1"};
        _taskRepositoryMock.Setup(x => x.Update(task));
        
        // Act
        var exception = Record.Exception(() => _taskService.Update(task));
        
        // Assert
        Assert.Null(exception);
    }
    
    [Fact]
    public void Delete_DeletesTask()
    {
        // Arrange
        var task = new TaskModel {Id = 1, ListId = 1, Name = "Task 1", Description = "Description 1"};
        _taskRepositoryMock.Setup(x => x.Delete(1));
        
        // Act
        var exception = Record.Exception(() => _taskService.Delete(1));
        
        // Assert
        Assert.Null(exception);
    }
    
    [Fact]
    public void Delete_ThrowsException()
    {
        // Arrange
        var task = new TaskModel {Id = 1, ListId = 1, Name = "Task 1", Description = "Description 1"};
        _taskRepositoryMock.Setup(x => x.Delete(1)).Throws(new Exception());
        
        // Act and Assert
        Assert.Throws<Exception>(() => _taskService.Delete(1));
    }
    
    [Fact]
    public void DeleteByListId_DeletesTasks()
    {
        // Arrange
        var listId = 1;
        _taskRepositoryMock.Setup(x => x.DeleteByListId(listId));
        
        // Act
        var exception = Record.Exception(() => _taskService.DeleteByListId(listId));
        
        // Assert
        Assert.Null(exception);
    }
}