using Xunit;
using Moq;
using FluentAssertions;
using MotoklubBezbednost.API.Models;
using MotoklubBezbednost.API.Repositories;
using MotoklubBezbednost.API.Services;

namespace MotoklubBezbednost.API.Tests.Services;

public class EquipmentServiceTests
{
    private readonly Mock<IEquipmentRepository> _mockRepository;
    private readonly EquipmentService _equipmentService;

    public EquipmentServiceTests()
    {
        _mockRepository = new Mock<IEquipmentRepository>();
        _equipmentService = new EquipmentService(_mockRepository.Object);
    }

    [Fact]
    public async Task GetAllEquipmentAsync_ShouldReturnAllEquipment()
    {
        // Arrange
        var expectedEquipment = new List<Equipment>
        {
            new Equipment { Id = 1, MemberId = 1, Pants = true, Jacket = true },
            new Equipment { Id = 2, MemberId = 2, Pants = true, Jacket = false }
        };

        _mockRepository.Setup(r => r.GetAllWithMemberAsync())
            .ReturnsAsync(expectedEquipment);

        // Act
        var result = await _equipmentService.GetAllEquipmentAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(expectedEquipment);
        _mockRepository.Verify(r => r.GetAllWithMemberAsync(), Times.Once);
    }

    [Fact]
    public async Task GetEquipmentByIdAsync_WhenEquipmentExists_ShouldReturnEquipment()
    {
        // Arrange
        var equipmentId = 1;
        var expectedEquipment = new Equipment { Id = equipmentId, MemberId = 1, Pants = true, Jacket = true };

        _mockRepository.Setup(r => r.GetByIdAsync(equipmentId))
            .ReturnsAsync(expectedEquipment);

        // Act
        var result = await _equipmentService.GetEquipmentByIdAsync(equipmentId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedEquipment);
        _mockRepository.Verify(r => r.GetByIdAsync(equipmentId), Times.Once);
    }

    [Fact]
    public async Task GetEquipmentByIdAsync_WhenEquipmentDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var equipmentId = 999;
        _mockRepository.Setup(r => r.GetByIdAsync(equipmentId))
            .ReturnsAsync((Equipment?)null);

        // Act
        var result = await _equipmentService.GetEquipmentByIdAsync(equipmentId);

        // Assert
        result.Should().BeNull();
        _mockRepository.Verify(r => r.GetByIdAsync(equipmentId), Times.Once);
    }

    [Fact]
    public async Task GetEquipmentByMemberIdAsync_WhenEquipmentExists_ShouldReturnEquipment()
    {
        // Arrange
        var memberId = 1;
        var expectedEquipment = new Equipment { Id = 1, MemberId = memberId, Pants = true, Jacket = true };

        _mockRepository.Setup(r => r.GetByMemberIdAsync(memberId))
            .ReturnsAsync(expectedEquipment);

        // Act
        var result = await _equipmentService.GetEquipmentByMemberIdAsync(memberId);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().Should().BeEquivalentTo(expectedEquipment);
        _mockRepository.Verify(r => r.GetByMemberIdAsync(memberId), Times.Once);
    }

    [Fact]
    public async Task GetEquipmentByMemberIdAsync_WhenEquipmentDoesNotExist_ShouldReturnEmpty()
    {
        // Arrange
        var memberId = 999;
        _mockRepository.Setup(r => r.GetByMemberIdAsync(memberId))
            .ReturnsAsync((Equipment?)null);

        // Act
        var result = await _equipmentService.GetEquipmentByMemberIdAsync(memberId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
        _mockRepository.Verify(r => r.GetByMemberIdAsync(memberId), Times.Once);
    }

    [Fact]
    public async Task CreateEquipmentAsync_ShouldAddAndReturnEquipment()
    {
        // Arrange
        var newEquipment = new Equipment { MemberId = 1, Pants = true, Jacket = true };
        var createdEquipment = new Equipment { Id = 1, MemberId = 1, Pants = true, Jacket = true };

        _mockRepository.Setup(r => r.AddAsync(newEquipment))
            .ReturnsAsync(createdEquipment);

        // Act
        var result = await _equipmentService.CreateEquipmentAsync(newEquipment);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Pants.Should().BeTrue();
        _mockRepository.Verify(r => r.AddAsync(newEquipment), Times.Once);
    }

    [Fact]
    public async Task UpdateEquipmentAsync_ShouldUpdateEquipment()
    {
        // Arrange
        var equipment = new Equipment { Id = 1, MemberId = 1, Pants = false, Jacket = true };

        _mockRepository.Setup(r => r.UpdateAsync(equipment))
            .Returns(Task.CompletedTask);

        // Act
        await _equipmentService.UpdateEquipmentAsync(equipment);

        // Assert
        _mockRepository.Verify(r => r.UpdateAsync(equipment), Times.Once);
    }

    [Fact]
    public async Task DeleteEquipmentAsync_ShouldDeleteEquipment()
    {
        // Arrange
        var equipmentId = 1;
        _mockRepository.Setup(r => r.DeleteAsync(equipmentId))
            .Returns(Task.CompletedTask);

        // Act
        await _equipmentService.DeleteEquipmentAsync(equipmentId);

        // Assert
        _mockRepository.Verify(r => r.DeleteAsync(equipmentId), Times.Once);
    }
}

