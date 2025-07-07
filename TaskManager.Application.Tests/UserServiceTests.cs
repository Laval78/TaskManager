using Moq;
using TaskManager.Application.DTO.User;
using TaskManager.Domain.Models;
using TaskManager.Infrastructure.Services;

namespace TaskManager.Application.Tests;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly UserService _userService;


    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userService = new UserService(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldCallRepository_WithCorrectUser()
    {
        // Arrange
        var dto = new PostUserDto { Name = "Test User" };

        // Act
        await _userService.CreateAsync(dto);

        // Assert
        _userRepositoryMock.Verify(r => r.AddAsync(It.Is<User>(
            u => u.Name == dto.Name
        )), Times.Once);
    }
}