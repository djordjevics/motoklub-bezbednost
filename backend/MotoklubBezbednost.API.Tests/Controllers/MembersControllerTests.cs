using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using FluentAssertions;
using MotoklubBezbednost.API.Controllers;
using MotoklubBezbednost.API.Models;
using MotoklubBezbednost.API.Services;

namespace MotoklubBezbednost.API.Tests.Controllers;

public class MembersControllerTests
{
    private readonly Mock<IMemberService> _mockService;
    private readonly MembersController _controller;

    public MembersControllerTests()
    {
        _mockService = new Mock<IMemberService>();
        _controller = new MembersController(_mockService.Object);
    }

    [Fact]
    public async Task GetMembers_ShouldReturnOkResultWithMembers()
    {
        // Arrange
        var members = new List<Member>
        {
            new Member { Id = 1, Name = "John", Surname = "Doe" },
            new Member { Id = 2, Name = "Jane", Surname = "Smith" }
        };

        _mockService.Setup(s => s.GetAllMembersAsync())
            .ReturnsAsync(members);

        // Act
        var result = await _controller.GetMembers();

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(members);
    }

    [Fact]
    public async Task GetMember_WhenMemberExists_ShouldReturnOkResult()
    {
        // Arrange
        var memberId = 1;
        var member = new Member { Id = memberId, Name = "John", Surname = "Doe" };

        _mockService.Setup(s => s.GetMemberByIdAsync(memberId))
            .ReturnsAsync(member);

        // Act
        var result = await _controller.GetMember(memberId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(member);
    }

    [Fact]
    public async Task GetMember_WhenMemberDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var memberId = 999;
        _mockService.Setup(s => s.GetMemberByIdAsync(memberId))
            .ReturnsAsync((Member?)null);

        // Act
        var result = await _controller.GetMember(memberId);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task CreateMember_ShouldReturnCreatedAtAction()
    {
        // Arrange
        var member = new Member { Name = "John", Surname = "Doe" };
        var createdMember = new Member { Id = 1, Name = "John", Surname = "Doe" };

        _mockService.Setup(s => s.CreateMemberAsync(member))
            .ReturnsAsync(createdMember);

        // Act
        var result = await _controller.CreateMember(member);

        // Assert
        var createdAtActionResult = result.Result as CreatedAtActionResult;
        createdAtActionResult.Should().NotBeNull();
        createdAtActionResult!.Value.Should().BeEquivalentTo(createdMember);
        createdAtActionResult.ActionName.Should().Be(nameof(_controller.GetMember));
    }

    [Fact]
    public async Task UpdateMember_ShouldReturnNoContent()
    {
        // Arrange
        var memberId = 1;
        var member = new Member { Id = memberId, Name = "John", Surname = "Doe Updated" };

        _mockService.Setup(s => s.UpdateMemberAsync(member))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateMember(memberId, member);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task UpdateMember_WhenIdMismatch_ShouldReturnBadRequest()
    {
        // Arrange
        var memberId = 1;
        var member = new Member { Id = 2, Name = "John", Surname = "Doe" };

        // Act
        var result = await _controller.UpdateMember(memberId, member);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public async Task DeleteMember_ShouldReturnNoContent()
    {
        // Arrange
        var memberId = 1;
        _mockService.Setup(s => s.DeleteMemberAsync(memberId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteMember(memberId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task SearchMembers_ShouldReturnOkResultWithMatchingMembers()
    {
        // Arrange
        var query = "John";
        var members = new List<Member>
        {
            new Member { Id = 1, Name = "John", Surname = "Doe" }
        };

        _mockService.Setup(s => s.SearchMembersAsync(query))
            .ReturnsAsync(members);

        // Act
        var result = await _controller.SearchMembers(query);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(members);
    }
}

