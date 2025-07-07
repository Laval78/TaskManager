using Moq;
using TaskManager.Application.DTO.TaskList;
using TaskManager.Application.Exceptions;
using TaskManager.Domain.Models;
using TaskManager.Infrastructure.Services;

namespace TaskManager.Application.Tests;

public class TaskListServiceTests
{
    private readonly Mock<ITaskListRepository> _repositoryMock;
    private readonly TaskListService _service;

    public TaskListServiceTests()
    {
        _repositoryMock = new Mock<ITaskListRepository>();
        _service = new TaskListService(_repositoryMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnNewTaskListId()
    {
        var dto = new PostTaskListDto { Title = "Тестовий список" };
        int userId = 42;
        _repositoryMock
            .Setup(r => r.CreateAsync(It.IsAny<TaskList>()))
            .ReturnsAsync(999);

        var result = await _service.CreateAsync(dto, userId);

        Assert.Equal(999, result);
        _repositoryMock.Verify(r => r.CreateAsync(It.Is<TaskList>(
            t => t.Title == dto.Title && t.WhoCreated == userId
        )), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsCorrectList()
    {
        int userId = 1;
        int page = 1;
        int pageSize = 2;

        var taskLists = new List<Domain.Models.TaskList>
    {
        new() { Id = 1, Title = "List 1", WhoCreated = userId },
        new() { Id = 2, Title = "List 2", WhoCreated = userId }
    };

        _repositoryMock
            .Setup(r => r.GetAllForUserAsync(userId, page, pageSize))
            .ReturnsAsync(taskLists);

        var result = await _service.GetAllAsync(userId, page, pageSize);

        Assert.Equal(2, result.Count());
        Assert.Contains(result, r => r.Name == "List 1");
    }

    [Fact]
    public async Task GetByIdAsync_ThrowsNotFound_WhenNotExists()
    {
        int id = 100;
        int userId = 1;

        _repositoryMock
            .Setup(r => r.GetByIdWithCreatorAsync(id, userId))
            .ReturnsAsync((Domain.Models.TaskList?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _service.GetByIdAsync(id, userId));
    }

    [Fact]
    public async Task UpdateAsync_ThrowsForbidden_WhenNotOwner()
    {
        int id = 1;
        int userId = 999;
        var dto = new PutTaskListDto { Title = "Iм'я" };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(new Domain.Models.TaskList { Id = id, WhoCreated = 123 });

        await Assert.ThrowsAsync<ForbiddenException>(() => _service.UpdateAsync(id, dto, userId));
    }

    [Fact]
    public async Task DeleteAsync_CallsDelete_WhenOwner()
    {
        int id = 1;
        int userId = 123;
        var taskList = new Domain.Models.TaskList { Id = id, WhoCreated = userId };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(id))
            .ReturnsAsync(taskList);

        _repositoryMock
            .Setup(r => r.DeleteAsync(taskList))
            .ReturnsAsync(true);

        await _service.DeleteAsync(id, userId);

        _repositoryMock.Verify(r => r.DeleteAsync(taskList), Times.Once);
    }
}
