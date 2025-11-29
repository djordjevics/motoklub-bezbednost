using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using FluentAssertions;
using MotoklubBezbednost.API.Controllers;
using MotoklubBezbednost.API.Models;
using MotoklubBezbednost.API.Services;

namespace MotoklubBezbednost.API.Tests.Controllers;

public class EquipmentControllerTests
{
    private readonly Mock<IEquipmentService> _mockService;
    private readonly EquipmentController _controller;

    public EquipmentControllerTests()
    {
        _mockService = new Mock<IEquipmentService>();
        _controller = new EquipmentController(_mockService.Object);
    }

    [Fact]
    public async Task GetEquipment_ShouldReturnOkResultWithEquipment()
    {
        // Arrange
        var equipment = new List<Equipment>
        {
            new Equipment { Id = 1, MemberId = 1, Pants = true, Jacket = true },
            new Equipment { Id = 2, MemberId = 2, Pants = true, Jacket = false }
        };

        _mockService.Setup(s => s.GetAllEquipmentAsync())
            .ReturnsAsync(equipment);

        // Act
        var result = await _controller.GetEquipment();

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(equipment);
    }

    [Fact]
    public async Task GetEquipment_WhenEquipmentExists_ShouldReturnOkResult()
    {
        // Arrange
        var equipmentId = 1;
        var equipment = new Equipment { Id = equipmentId, MemberId = 1, Pants = true, Jacket = true };

        _mockService.Setup(s => s.GetEquipmentByIdAsync(equipmentId))
            .ReturnsAsync(equipment);

        // Act
        var result = await _controller.GetEquipment(equipmentId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(equipment);
    }

    [Fact]
    public async Task GetEquipment_WhenEquipmentDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var equipmentId = 999;
        _mockService.Setup(s => s.GetEquipmentByIdAsync(equipmentId))
            .ReturnsAsync((Equipment?)null);

        // Act
        var result = await _controller.GetEquipment(equipmentId);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetEquipmentByMember_ShouldReturnOkResultWithEquipment()
    {
        // Arrange
        var memberId = 1;
        var equipment = new List<Equipment>
        {
            new Equipment { Id = 1, MemberId = memberId, Pants = true, Jacket = true }
        };

        _mockService.Setup(s => s.GetEquipmentByMemberIdAsync(memberId))
            .ReturnsAsync(equipment);

        // Act
        var result = await _controller.GetEquipmentByMember(memberId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(equipment);
    }

    [Fact]
    public async Task CreateEquipment_ShouldReturnCreatedAtAction()
    {
        // Arrange
        var equipment = new Equipment { MemberId = 1, Pants = true, Jacket = true };
        var createdEquipment = new Equipment { Id = 1, MemberId = 1, Pants = true, Jacket = true };

        _mockService.Setup(s => s.CreateEquipmentAsync(equipment))
            .ReturnsAsync(createdEquipment);

        // Act
        var result = await _controller.CreateEquipment(equipment);

        // Assert
        var createdAtActionResult = result.Result as CreatedAtActionResult;
        createdAtActionResult.Should().NotBeNull();
        createdAtActionResult!.Value.Should().BeEquivalentTo(createdEquipment);
        createdAtActionResult.ActionName.Should().Be(nameof(_controller.GetEquipment));
    }

    [Fact]
    public async Task UpdateEquipment_ShouldReturnNoContent()
    {
        // Arrange
        var equipmentId = 1;
        var equipment = new Equipment { Id = equipmentId, MemberId = 1, Pants = false, Jacket = true };

        _mockService.Setup(s => s.UpdateEquipmentAsync(equipment))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateEquipment(equipmentId, equipment);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task UpdateEquipment_WhenIdMismatch_ShouldReturnBadRequest()
    {
        // Arrange
        var equipmentId = 1;
        var equipment = new Equipment { Id = 2, MemberId = 1, Pants = true, Jacket = true };

        // Act
        var result = await _controller.UpdateEquipment(equipmentId, equipment);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public async Task DeleteEquipment_ShouldReturnNoContent()
    {
        // Arrange
        var equipmentId = 1;
        _mockService.Setup(s => s.DeleteEquipmentAsync(equipmentId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteEquipment(equipmentId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }
}

