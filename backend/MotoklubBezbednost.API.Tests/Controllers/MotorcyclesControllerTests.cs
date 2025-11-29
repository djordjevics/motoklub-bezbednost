using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using FluentAssertions;
using MotoklubBezbednost.API.Controllers;
using MotoklubBezbednost.API.Models;
using MotoklubBezbednost.API.Services;

namespace MotoklubBezbednost.API.Tests.Controllers;

public class MotorcyclesControllerTests
{
    private readonly Mock<IMotorcycleService> _mockService;
    private readonly MotorcyclesController _controller;

    public MotorcyclesControllerTests()
    {
        _mockService = new Mock<IMotorcycleService>();
        _controller = new MotorcyclesController(_mockService.Object);
    }

    [Fact]
    public async Task GetMotorcycles_ShouldReturnOkResultWithMotorcycles()
    {
        // Arrange
        var member1 = new Member { Id = 1, Name = "John", Surname = "Doe" };
        var member2 = new Member { Id = 2, Name = "Jane", Surname = "Smith" };
        var motorcycles = new List<Motorcycle>
        {
            new Motorcycle { Id = 1, Member = member1, BrandName = "Honda", ModelName = "CBR600" },
            new Motorcycle { Id = 2, Member = member2, BrandName = "Yamaha", ModelName = "R1" }
        };

        _mockService.Setup(s => s.GetAllMotorcyclesAsync())
            .ReturnsAsync(motorcycles);

        // Act
        var result = await _controller.GetMotorcycles();

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(motorcycles);
    }

    [Fact]
    public async Task GetMotorcycle_WhenMotorcycleExists_ShouldReturnOkResult()
    {
        // Arrange
        var motorcycleId = 1;
        var member = new Member { Id = 1, Name = "John", Surname = "Doe" };
        var motorcycle = new Motorcycle { Id = motorcycleId, Member = member, BrandName = "Honda", ModelName = "CBR600" };

        _mockService.Setup(s => s.GetMotorcycleByIdAsync(motorcycleId))
            .ReturnsAsync(motorcycle);

        // Act
        var result = await _controller.GetMotorcycle(motorcycleId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(motorcycle);
    }

    [Fact]
    public async Task GetMotorcycle_WhenMotorcycleDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var motorcycleId = 999;
        _mockService.Setup(s => s.GetMotorcycleByIdAsync(motorcycleId))
            .ReturnsAsync((Motorcycle?)null);

        // Act
        var result = await _controller.GetMotorcycle(motorcycleId);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetMotorcyclesByMember_ShouldReturnOkResultWithMotorcycles()
    {
        // Arrange
        var memberId = 1;
        var member = new Member { Id = memberId, Name = "John", Surname = "Doe" };
        var motorcycles = new List<Motorcycle>
        {
            new Motorcycle { Id = 1, Member = member, BrandName = "Honda", ModelName = "CBR600" }
        };

        _mockService.Setup(s => s.GetMotorcyclesByMemberIdAsync(memberId))
            .ReturnsAsync(motorcycles);

        // Act
        var result = await _controller.GetMotorcyclesByMember(memberId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(motorcycles);
    }

    [Fact]
    public async Task CreateMotorcycle_ShouldReturnCreatedAtAction()
    {
        // Arrange
        var member = new Member { Id = 1, Name = "John", Surname = "Doe" };
        var motorcycle = new Motorcycle { Member = member, BrandName = "Honda", ModelName = "CBR600" };
        var createdMotorcycle = new Motorcycle { Id = 1, Member = member, BrandName = "Honda", ModelName = "CBR600" };

        _mockService.Setup(s => s.CreateMotorcycleAsync(motorcycle))
            .ReturnsAsync(createdMotorcycle);

        // Act
        var result = await _controller.CreateMotorcycle(motorcycle);

        // Assert
        var createdAtActionResult = result.Result as CreatedAtActionResult;
        createdAtActionResult.Should().NotBeNull();
        createdAtActionResult!.Value.Should().BeEquivalentTo(createdMotorcycle);
        createdAtActionResult.ActionName.Should().Be(nameof(_controller.GetMotorcycle));
    }

    [Fact]
    public async Task UpdateMotorcycle_ShouldReturnNoContent()
    {
        // Arrange
        var motorcycleId = 1;
        var member = new Member { Id = 1, Name = "John", Surname = "Doe" };
        var motorcycle = new Motorcycle { Id = motorcycleId, Member = member, BrandName = "Honda", ModelName = "CBR600 Updated" };

        _mockService.Setup(s => s.UpdateMotorcycleAsync(motorcycle))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateMotorcycle(motorcycleId, motorcycle);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task UpdateMotorcycle_WhenIdMismatch_ShouldReturnBadRequest()
    {
        // Arrange
        var motorcycleId = 1;
        var member = new Member { Id = 1, Name = "John", Surname = "Doe" };
        var motorcycle = new Motorcycle { Id = 2, Member = member, BrandName = "Honda", ModelName = "CBR600" };

        // Act
        var result = await _controller.UpdateMotorcycle(motorcycleId, motorcycle);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public async Task DeleteMotorcycle_ShouldReturnNoContent()
    {
        // Arrange
        var motorcycleId = 1;
        _mockService.Setup(s => s.DeleteMotorcycleAsync(motorcycleId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteMotorcycle(motorcycleId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }
}

