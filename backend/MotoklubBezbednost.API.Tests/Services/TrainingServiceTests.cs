using Xunit;
using Moq;
using FluentAssertions;
using MotoklubBezbednost.API.Models;
using MotoklubBezbednost.API.Repositories;
using MotoklubBezbednost.API.Services;

namespace MotoklubBezbednost.API.Tests.Services;

public class TrainingServiceTests
{
    private readonly Mock<ITrainingSessionRepository> _mockSessionRepository;
    private readonly Mock<ITrainingRepository> _mockTrainingRepository;
    private readonly TrainingService _trainingService;

    public TrainingServiceTests()
    {
        _mockSessionRepository = new Mock<ITrainingSessionRepository>();
        _mockTrainingRepository = new Mock<ITrainingRepository>();
        _trainingService = new TrainingService(_mockSessionRepository.Object, _mockTrainingRepository.Object);
    }

    [Fact]
    public async Task GetAllTrainingSessionsAsync_ShouldReturnAllSessions()
    {
        // Arrange
        var expectedSessions = new List<TrainingSession>
        {
            new TrainingSession { Id = 1, City = "Belgrade", TheoryDate = DateTime.Now },
            new TrainingSession { Id = 2, City = "Novi Sad", TheoryDate = DateTime.Now.AddDays(1) }
        };

        _mockSessionRepository.Setup(r => r.GetAllWithDetailsAsync())
            .ReturnsAsync(expectedSessions);

        // Act
        var result = await _trainingService.GetAllTrainingSessionsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(expectedSessions);
        _mockSessionRepository.Verify(r => r.GetAllWithDetailsAsync(), Times.Once);
    }

    [Fact]
    public async Task GetTrainingSessionByIdAsync_WhenSessionExists_ShouldReturnSession()
    {
        // Arrange
        var sessionId = 1;
        var expectedSession = new TrainingSession { Id = sessionId, City = "Belgrade", TheoryDate = DateTime.Now };

        _mockSessionRepository.Setup(r => r.GetByIdWithDetailsAsync(sessionId))
            .ReturnsAsync(expectedSession);

        // Act
        var result = await _trainingService.GetTrainingSessionByIdAsync(sessionId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedSession);
        _mockSessionRepository.Verify(r => r.GetByIdWithDetailsAsync(sessionId), Times.Once);
    }

    [Fact]
    public async Task GetTrainingSessionByIdAsync_WhenSessionDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var sessionId = 999;
        _mockSessionRepository.Setup(r => r.GetByIdWithDetailsAsync(sessionId))
            .ReturnsAsync((TrainingSession?)null);

        // Act
        var result = await _trainingService.GetTrainingSessionByIdAsync(sessionId);

        // Assert
        result.Should().BeNull();
        _mockSessionRepository.Verify(r => r.GetByIdWithDetailsAsync(sessionId), Times.Once);
    }

    [Fact]
    public async Task CreateTrainingSessionAsync_ShouldAddAndReturnSession()
    {
        // Arrange
        var newSession = new TrainingSession { City = "Belgrade", TheoryDate = DateTime.Now };
        var createdSession = new TrainingSession { Id = 1, City = "Belgrade", TheoryDate = DateTime.Now };

        _mockSessionRepository.Setup(r => r.AddAsync(newSession))
            .ReturnsAsync(createdSession);

        // Act
        var result = await _trainingService.CreateTrainingSessionAsync(newSession);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.City.Should().Be("Belgrade");
        _mockSessionRepository.Verify(r => r.AddAsync(newSession), Times.Once);
    }

    [Fact]
    public async Task UpdateTrainingSessionAsync_ShouldUpdateSession()
    {
        // Arrange
        var session = new TrainingSession { Id = 1, City = "Belgrade Updated", TheoryDate = DateTime.Now };

        _mockSessionRepository.Setup(r => r.UpdateAsync(session))
            .Returns(Task.CompletedTask);

        // Act
        await _trainingService.UpdateTrainingSessionAsync(session);

        // Assert
        _mockSessionRepository.Verify(r => r.UpdateAsync(session), Times.Once);
    }

    [Fact]
    public async Task DeleteTrainingSessionAsync_ShouldDeleteSession()
    {
        // Arrange
        var sessionId = 1;
        _mockSessionRepository.Setup(r => r.DeleteAsync(sessionId))
            .Returns(Task.CompletedTask);

        // Act
        await _trainingService.DeleteTrainingSessionAsync(sessionId);

        // Assert
        _mockSessionRepository.Verify(r => r.DeleteAsync(sessionId), Times.Once);
    }

    [Fact]
    public async Task GetTrainingsByMemberIdAsync_ShouldReturnMemberTrainings()
    {
        // Arrange
        var memberId = 1;
        var expectedTrainings = new List<Training>
        {
            new Training { Id = 1, MemberId = memberId, MotorcycleId = 1, TrainingSessionId = 1 }
        };

        _mockTrainingRepository.Setup(r => r.GetByMemberIdAsync(memberId))
            .ReturnsAsync(expectedTrainings);

        // Act
        var result = await _trainingService.GetTrainingsByMemberIdAsync(memberId);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.Should().BeEquivalentTo(expectedTrainings);
        _mockTrainingRepository.Verify(r => r.GetByMemberIdAsync(memberId), Times.Once);
    }

    [Fact]
    public async Task GetTrainingByIdAsync_WhenTrainingExists_ShouldReturnTraining()
    {
        // Arrange
        var trainingId = 1;
        var expectedTraining = new Training { Id = trainingId, MemberId = 1, MotorcycleId = 1, TrainingSessionId = 1 };

        _mockTrainingRepository.Setup(r => r.GetByIdWithDetailsAsync(trainingId))
            .ReturnsAsync(expectedTraining);

        // Act
        var result = await _trainingService.GetTrainingByIdAsync(trainingId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedTraining);
        _mockTrainingRepository.Verify(r => r.GetByIdWithDetailsAsync(trainingId), Times.Once);
    }

    [Fact]
    public async Task CreateTrainingAsync_ShouldAddAndReturnTraining()
    {
        // Arrange
        var newTraining = new Training { MemberId = 1, MotorcycleId = 1, TrainingSessionId = 1 };
        var createdTraining = new Training { Id = 1, MemberId = 1, MotorcycleId = 1, TrainingSessionId = 1 };

        _mockTrainingRepository.Setup(r => r.AddAsync(newTraining))
            .ReturnsAsync(createdTraining);

        // Act
        var result = await _trainingService.CreateTrainingAsync(newTraining);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        _mockTrainingRepository.Verify(r => r.AddAsync(newTraining), Times.Once);
    }
}

