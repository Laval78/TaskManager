using Moq;
using TaskManager.Application.DTO.TaskListUser;
using TaskManager.Application.Exceptions;
using TaskManager.Application.Interfaces.Repositories;
using TaskManager.Domain.Models;
using TaskManager.Infrastructure.Services;

namespace TaskManager.Application.Tests;

public class TaskListUserServiceTests
{
    private readonly Mock<ITaskListUserRepository> _repoMock;
    private readonly TaskListUserService _service;

    public TaskListUserServiceTests()
    {
        _repoMock = new Mock<ITaskListUserRepository>();
        _service = new TaskListUserService(_repoMock.Object, null!);
    }

    [Fact]
    public async Task AddUserToTaskListAsync_ShouldThrow_WhenTaskListNotFound()
    {
        _repoMock.Setup(r => r.GetTaskListWithUsersAsync(1)).ReturnsAsync((TaskList?)null);

        var dto = new PostTaskListUserDto { UserId = 2 };

        await Assert.ThrowsAsync<NotFoundException>(() => _service.AddUserToTaskListAsync(1, dto, 1));
    }

    [Fact]
    public async Task AddUserToTaskListAsync_ShouldThrow_WhenUserTriesToAddThemself()
    {
        var taskList = new TaskList { Id = 1, WhoCreated = 1, TaskListUsers = new List<TaskListUser>() };
        _repoMock.Setup(r => r.GetTaskListWithUsersAsync(1)).ReturnsAsync(taskList);

        var dto = new PostTaskListUserDto { UserId = 1 };

        await Assert.ThrowsAsync<BadRequestException>(() => _service.AddUserToTaskListAsync(1, dto, 1));
    }

    [Fact]
    public async Task AddUserToTaskListAsync_ShouldThrow_WhenUserNotOwner()
    {
        var taskList = new TaskList { Id = 1, WhoCreated = 1, TaskListUsers = new List<TaskListUser>() };
        _repoMock.Setup(r => r.GetTaskListWithUsersAsync(1)).ReturnsAsync(taskList);

        var dto = new PostTaskListUserDto { UserId = 2 };

        await Assert.ThrowsAsync<ForbiddenException>(() => _service.AddUserToTaskListAsync(1, dto, 999));
    }

    [Fact]
    public async Task AddUserToTaskListAsync_ShouldThrow_WhenUserAlreadyExists()
    {
        var taskList = new TaskList
        {
            Id = 1,
            WhoCreated = 1,
            TaskListUsers = new List<TaskListUser>
            {
                new TaskListUser { IdUser = 2 }
            }
        };
        _repoMock.Setup(r => r.GetTaskListWithUsersAsync(1)).ReturnsAsync(taskList);

        var dto = new PostTaskListUserDto { UserId = 2 };

        await Assert.ThrowsAsync<BadRequestException>(() => _service.AddUserToTaskListAsync(1, dto, 1));
    }

    [Fact]
    public async Task AddUserToTaskListAsync_ShouldThrow_WhenMoreThanThreeUsers()
    {
        var taskList = new TaskList
        {
            Id = 1,
            WhoCreated = 1,
            TaskListUsers = new List<TaskListUser>
            {
                new TaskListUser(),
                new TaskListUser(),
                new TaskListUser()
            }
        };
        _repoMock.Setup(r => r.GetTaskListWithUsersAsync(1)).ReturnsAsync(taskList);

        var dto = new PostTaskListUserDto { UserId = 4 };

        await Assert.ThrowsAsync<BadRequestException>(() => _service.AddUserToTaskListAsync(1, dto, 1));
    }

    [Fact]
    public async Task AddUserToTaskListAsync_ShouldSucceed_WhenValid()
    {
        var taskList = new TaskList
        {
            Id = 1,
            WhoCreated = 1,
            TaskListUsers = new List<TaskListUser>()
        };
        _repoMock.Setup(r => r.GetTaskListWithUsersAsync(1)).ReturnsAsync(taskList);

        var dto = new PostTaskListUserDto { UserId = 2 };

        var result = await _service.AddUserToTaskListAsync(1, dto, 1);

        Assert.True(result);
        _repoMock.Verify(r => r.AddUserToTaskListAsync(It.IsAny<TaskListUser>()), Times.Once);
    }
}
