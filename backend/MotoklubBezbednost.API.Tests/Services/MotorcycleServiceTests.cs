using Xunit;
using Moq;
using FluentAssertions;
using MotoklubBezbednost.API.Models;
using MotoklubBezbednost.API.Repositories;
using MotoklubBezbednost.API.Services;

namespace MotoklubBezbednost.API.Tests.Services;

public class MotorcycleServiceTests
{
    private readonly Mock<IMotorcycleRepository> _mockRepository;
    private readonly MotorcycleService _motorcycleService;

    public MotorcycleServiceTests()
    {
        _mockRepository = new Mock<IMotorcycleRepository>();
        _motorcycleService = new MotorcycleService(_mockRepository.Object);
    }

    [Fact]
    public async Task GetAllMotorcyclesAsync_ShouldReturnAllMotorcycles()
    {
        // Arrange
        var expectedMotorcycles = new List<Motorcycle>
        {
            new Motorcycle { Id = 1, MemberId = 1, BrandName = "Honda", ModelName = "CBR600" },
            new Motorcycle { Id = 2, MemberId = 2, BrandName = "Yamaha", ModelName = "R1" }
        };

        _mockRepository.Setup(r => r.GetAllWithMemberAsync())
            .ReturnsAsync(expectedMotorcycles);

        // Act
        var result = await _motorcycleService.GetAllMotorcyclesAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(expectedMotorcycles);
        _mockRepository.Verify(r => r.GetAllWithMemberAsync(), Times.Once);
    }

    [Fact]
    public async Task GetMotorcycleByIdAsync_WhenMotorcycleExists_ShouldReturnMotorcycle()
    {
        // Arrange
        var motorcycleId = 1;
        var expectedMotorcycle = new Motorcycle { Id = motorcycleId, MemberId = 1, BrandName = "Honda", ModelName = "CBR600" };

        _mockRepository.Setup(r => r.GetByIdAsync(motorcycleId))
            .ReturnsAsync(expectedMotorcycle);

        // Act
        var result = await _motorcycleService.GetMotorcycleByIdAsync(motorcycleId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedMotorcycle);
        _mockRepository.Verify(r => r.GetByIdAsync(motorcycleId), Times.Once);
    }

    [Fact]
    public async Task GetMotorcycleByIdAsync_WhenMotorcycleDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var motorcycleId = 999;
        _mockRepository.Setup(r => r.GetByIdAsync(motorcycleId))
            .ReturnsAsync((Motorcycle?)null);

        // Act
        var result = await _motorcycleService.GetMotorcycleByIdAsync(motorcycleId);

        // Assert
        result.Should().BeNull();
        _mockRepository.Verify(r => r.GetByIdAsync(motorcycleId), Times.Once);
    }

    [Fact]
    public async Task GetMotorcyclesByMemberIdAsync_ShouldReturnMemberMotorcycles()
    {
        // Arrange
        var memberId = 1;
        var expectedMotorcycles = new List<Motorcycle>
        {
            new Motorcycle { Id = 1, MemberId = memberId, BrandName = "Honda", ModelName = "CBR600" }
        };

        _mockRepository.Setup(r => r.GetByMemberIdAsync(memberId))
            .ReturnsAsync(expectedMotorcycles);

        // Act
        var result = await _motorcycleService.GetMotorcyclesByMemberIdAsync(memberId);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.Should().BeEquivalentTo(expectedMotorcycles);
        _mockRepository.Verify(r => r.GetByMemberIdAsync(memberId), Times.Once);
    }

    [Fact]
    public async Task CreateMotorcycleAsync_ShouldAddAndReturnMotorcycle()
    {
        // Arrange
        var newMotorcycle = new Motorcycle { MemberId = 1, BrandName = "Honda", ModelName = "CBR600" };
        var createdMotorcycle = new Motorcycle { Id = 1, MemberId = 1, BrandName = "Honda", ModelName = "CBR600" };

        _mockRepository.Setup(r => r.AddAsync(newMotorcycle))
            .ReturnsAsync(createdMotorcycle);

        // Act
        var result = await _motorcycleService.CreateMotorcycleAsync(newMotorcycle);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.BrandName.Should().Be("Honda");
        _mockRepository.Verify(r => r.AddAsync(newMotorcycle), Times.Once);
    }

    [Fact]
    public async Task UpdateMotorcycleAsync_ShouldUpdateMotorcycle()
    {
        // Arrange
        var motorcycle = new Motorcycle { Id = 1, MemberId = 1, BrandName = "Honda", ModelName = "CBR600 Updated" };

        _mockRepository.Setup(r => r.UpdateAsync(motorcycle))
            .Returns(Task.CompletedTask);

        // Act
        await _motorcycleService.UpdateMotorcycleAsync(motorcycle);

        // Assert
        _mockRepository.Verify(r => r.UpdateAsync(motorcycle), Times.Once);
    }

    [Fact]
    public async Task DeleteMotorcycleAsync_ShouldDeleteMotorcycle()
    {
        // Arrange
        var motorcycleId = 1;
        _mockRepository.Setup(r => r.DeleteAsync(motorcycleId))
            .Returns(Task.CompletedTask);

        // Act
        await _motorcycleService.DeleteMotorcycleAsync(motorcycleId);

        // Assert
        _mockRepository.Verify(r => r.DeleteAsync(motorcycleId), Times.Once);
    }
}

