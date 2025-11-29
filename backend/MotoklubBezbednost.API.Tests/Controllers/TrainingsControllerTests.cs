using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using FluentAssertions;
using MotoklubBezbednost.API.Controllers;
using MotoklubBezbednost.API.Models;
using MotoklubBezbednost.API.Services;

namespace MotoklubBezbednost.API.Tests.Controllers;

public class TrainingsControllerTests
{
    private readonly Mock<ITrainingService> _mockService;
    private readonly TrainingsController _controller;

    public TrainingsControllerTests()
    {
        _mockService = new Mock<ITrainingService>();
        _controller = new TrainingsController(_mockService.Object);
    }

    [Fact]
    public async Task GetTrainingSessions_ShouldReturnOkResultWithSessions()
    {
        // Arrange
        var sessions = new List<TrainingSession>
        {
            new TrainingSession { Id = 1, City = "Belgrade", TheoryDate = DateTime.Now },
            new TrainingSession { Id = 2, City = "Novi Sad", TheoryDate = DateTime.Now.AddDays(1) }
        };

        _mockService.Setup(s => s.GetAllTrainingSessionsAsync())
            .ReturnsAsync(sessions);

        // Act
        var result = await _controller.GetTrainingSessions();

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(sessions);
    }

    [Fact]
    public async Task GetTrainingSession_WhenSessionExists_ShouldReturnOkResult()
    {
        // Arrange
        var sessionId = 1;
        var session = new TrainingSession { Id = sessionId, City = "Belgrade", TheoryDate = DateTime.Now };

        _mockService.Setup(s => s.GetTrainingSessionByIdAsync(sessionId))
            .ReturnsAsync(session);

        // Act
        var result = await _controller.GetTrainingSession(sessionId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(session);
    }

    [Fact]
    public async Task GetTrainingSession_WhenSessionDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var sessionId = 999;
        _mockService.Setup(s => s.GetTrainingSessionByIdAsync(sessionId))
            .ReturnsAsync((TrainingSession?)null);

        // Act
        var result = await _controller.GetTrainingSession(sessionId);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetTrainingsByMember_ShouldReturnOkResultWithTrainings()
    {
        // Arrange
        var memberId = 1;
        var member = new Member { Id = memberId, Name = "John", Surname = "Doe" };
        var motorcycle = new Motorcycle { Id = 1, Member = member, BrandName = "Honda", ModelName = "CBR600" };
        var session = new TrainingSession { Id = 1, City = "Belgrade", TheoryDate = DateTime.Now };
        var trainings = new List<Training>
        {
            new Training { Id = 1, Member = member, Motorcycle = motorcycle, TrainingSession = session }
        };

        _mockService.Setup(s => s.GetTrainingsByMemberIdAsync(memberId))
            .ReturnsAsync(trainings);

        // Act
        var result = await _controller.GetTrainingsByMember(memberId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(trainings);
    }

    [Fact]
    public async Task CreateTrainingSession_ShouldReturnCreatedAtAction()
    {
        // Arrange
        var session = new TrainingSession { City = "Belgrade", TheoryDate = DateTime.Now };
        var createdSession = new TrainingSession { Id = 1, City = "Belgrade", TheoryDate = DateTime.Now };

        _mockService.Setup(s => s.CreateTrainingSessionAsync(session))
            .ReturnsAsync(createdSession);

        // Act
        var result = await _controller.CreateTrainingSession(session);

        // Assert
        var createdAtActionResult = result.Result as CreatedAtActionResult;
        createdAtActionResult.Should().NotBeNull();
        createdAtActionResult!.Value.Should().BeEquivalentTo(createdSession);
        createdAtActionResult.ActionName.Should().Be(nameof(_controller.GetTrainingSession));
    }

    [Fact]
    public async Task CreateTraining_ShouldReturnCreatedAtAction()
    {
        // Arrange
        var member = new Member { Id = 1, Name = "John", Surname = "Doe" };
        var motorcycle = new Motorcycle { Id = 1, Member = member, BrandName = "Honda", ModelName = "CBR600" };
        var session = new TrainingSession { Id = 1, City = "Belgrade", TheoryDate = DateTime.Now };
        var training = new Training { Member = member, Motorcycle = motorcycle, TrainingSession = session };
        var createdTraining = new Training { Id = 1, Member = member, Motorcycle = motorcycle, TrainingSession = session };

        _mockService.Setup(s => s.CreateTrainingAsync(training))
            .ReturnsAsync(createdTraining);

        // Act
        var result = await _controller.CreateTraining(training);

        // Assert
        var createdAtActionResult = result.Result as CreatedAtActionResult;
        createdAtActionResult.Should().NotBeNull();
        createdAtActionResult!.Value.Should().BeEquivalentTo(createdTraining);
        createdAtActionResult.ActionName.Should().Be(nameof(_controller.GetTraining));
    }

    [Fact]
    public async Task GetTraining_WhenTrainingExists_ShouldReturnOkResult()
    {
        // Arrange
        var trainingId = 1;
        var member = new Member { Id = 1, Name = "John", Surname = "Doe" };
        var motorcycle = new Motorcycle { Id = 1, Member = member, BrandName = "Honda", ModelName = "CBR600" };
        var session = new TrainingSession { Id = 1, City = "Belgrade", TheoryDate = DateTime.Now };
        var training = new Training { Id = trainingId, Member = member, Motorcycle = motorcycle, TrainingSession = session };

        _mockService.Setup(s => s.GetTrainingByIdAsync(trainingId))
            .ReturnsAsync(training);

        // Act
        var result = await _controller.GetTraining(trainingId);

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.Value.Should().BeEquivalentTo(training);
    }

    [Fact]
    public async Task GetTraining_WhenTrainingDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var trainingId = 999;
        _mockService.Setup(s => s.GetTrainingByIdAsync(trainingId))
            .ReturnsAsync((Training?)null);

        // Act
        var result = await _controller.GetTraining(trainingId);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task UpdateTrainingSession_ShouldReturnNoContent()
    {
        // Arrange
        var sessionId = 1;
        var session = new TrainingSession { Id = sessionId, City = "Belgrade Updated", TheoryDate = DateTime.Now };

        _mockService.Setup(s => s.UpdateTrainingSessionAsync(session))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateTrainingSession(sessionId, session);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task UpdateTrainingSession_WhenIdMismatch_ShouldReturnBadRequest()
    {
        // Arrange
        var sessionId = 1;
        var session = new TrainingSession { Id = 2, City = "Belgrade", TheoryDate = DateTime.Now };

        // Act
        var result = await _controller.UpdateTrainingSession(sessionId, session);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public async Task DeleteTrainingSession_ShouldReturnNoContent()
    {
        // Arrange
        var sessionId = 1;
        _mockService.Setup(s => s.DeleteTrainingSessionAsync(sessionId))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteTrainingSession(sessionId);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }
}

