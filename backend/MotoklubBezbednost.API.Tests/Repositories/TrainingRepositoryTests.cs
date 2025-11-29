using Microsoft.EntityFrameworkCore;
using Xunit;
using FluentAssertions;
using MotoklubBezbednost.API.Data;
using MotoklubBezbednost.API.Models;
using MotoklubBezbednost.API.Repositories;

namespace MotoklubBezbednost.API.Tests.Repositories;

public class TrainingRepositoryTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly TrainingRepository _repository;

    public TrainingRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new TrainingRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddTrainingToDatabase()
    {
        // Arrange
        var member = new Member { Name = "John", Surname = "Doe" };
        var motorcycle = new Motorcycle { MemberId = member.Id, BrandName = "Honda", ModelName = "CBR600" };
        var level = new Level { Name = "Beginner" };
        var session = new TrainingSession { LevelId = level.Id, City = "Belgrade", TheoryDate = DateTime.Now };

        _context.Members.Add(member);
        _context.Motorcycles.Add(motorcycle);
        _context.Levels.Add(level);
        _context.TrainingSessions.Add(session);
        await _context.SaveChangesAsync();

        var training = new Training
        {
            MemberId = member.Id,
            MotorcycleId = motorcycle.Id,
            TrainingSessionId = session.Id,
            IsCertificateIssued = false
        };

        // Act
        var result = await _repository.AddAsync(training);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        var trainingInDb = await _context.Trainings.FindAsync(result.Id);
        trainingInDb.Should().NotBeNull();
    }

    [Fact]
    public async Task GetByIdAsync_WhenTrainingExists_ShouldReturnTraining()
    {
        // Arrange
        var member = new Member { Name = "John", Surname = "Doe" };
        var motorcycle = new Motorcycle { MemberId = member.Id, BrandName = "Honda", ModelName = "CBR600" };
        var level = new Level { Name = "Beginner" };
        var session = new TrainingSession { LevelId = level.Id, City = "Belgrade", TheoryDate = DateTime.Now };

        _context.Members.Add(member);
        _context.Motorcycles.Add(motorcycle);
        _context.Levels.Add(level);
        _context.TrainingSessions.Add(session);
        await _context.SaveChangesAsync();

        var training = new Training { MemberId = member.Id, MotorcycleId = motorcycle.Id, TrainingSessionId = session.Id };
        await _repository.AddAsync(training);

        // Act
        var result = await _repository.GetByIdAsync(training.Id);

        // Assert
        result.Should().NotBeNull();
        result!.MemberId.Should().Be(member.Id);
    }

    [Fact]
    public async Task GetByIdWithDetailsAsync_ShouldReturnTrainingWithDetails()
    {
        // Arrange
        var member = new Member { Name = "John", Surname = "Doe" };
        var motorcycle = new Motorcycle { MemberId = member.Id, BrandName = "Honda", ModelName = "CBR600" };
        var level = new Level { Name = "Beginner" };
        var session = new TrainingSession { LevelId = level.Id, City = "Belgrade", TheoryDate = DateTime.Now };

        _context.Members.Add(member);
        _context.Motorcycles.Add(motorcycle);
        _context.Levels.Add(level);
        _context.TrainingSessions.Add(session);
        await _context.SaveChangesAsync();

        var training = new Training { MemberId = member.Id, MotorcycleId = motorcycle.Id, TrainingSessionId = session.Id };
        await _repository.AddAsync(training);

        // Act
        var result = await _repository.GetByIdWithDetailsAsync(training.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Member.Should().NotBeNull();
        result.Motorcycle.Should().NotBeNull();
        result.TrainingSession.Should().NotBeNull();
        result.TrainingSession.Level.Should().NotBeNull();
    }

    [Fact]
    public async Task GetByMemberIdAsync_ShouldReturnMemberTrainings()
    {
        // Arrange
        var member1 = new Member { Name = "John", Surname = "Doe" };
        var member2 = new Member { Name = "Jane", Surname = "Smith" };
        var motorcycle1 = new Motorcycle { MemberId = member1.Id, BrandName = "Honda", ModelName = "CBR600" };
        var motorcycle2 = new Motorcycle { MemberId = member2.Id, BrandName = "Yamaha", ModelName = "R1" };
        var level = new Level { Name = "Beginner" };
        var session = new TrainingSession { LevelId = level.Id, City = "Belgrade", TheoryDate = DateTime.Now };

        _context.Members.AddRange(member1, member2);
        _context.Motorcycles.AddRange(motorcycle1, motorcycle2);
        _context.Levels.Add(level);
        _context.TrainingSessions.Add(session);
        await _context.SaveChangesAsync();

        await _repository.AddAsync(new Training { MemberId = member1.Id, MotorcycleId = motorcycle1.Id, TrainingSessionId = session.Id });
        await _repository.AddAsync(new Training { MemberId = member1.Id, MotorcycleId = motorcycle1.Id, TrainingSessionId = session.Id });
        await _repository.AddAsync(new Training { MemberId = member2.Id, MotorcycleId = motorcycle2.Id, TrainingSessionId = session.Id });

        // Act
        var result = await _repository.GetByMemberIdAsync(member1.Id);

        // Assert
        result.Should().HaveCount(2);
        result.All(t => t.MemberId == member1.Id).Should().BeTrue();
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateTraining()
    {
        // Arrange
        var member = new Member { Name = "John", Surname = "Doe" };
        var motorcycle = new Motorcycle { MemberId = member.Id, BrandName = "Honda", ModelName = "CBR600" };
        var level = new Level { Name = "Beginner" };
        var session = new TrainingSession { LevelId = level.Id, City = "Belgrade", TheoryDate = DateTime.Now };

        _context.Members.Add(member);
        _context.Motorcycles.Add(motorcycle);
        _context.Levels.Add(level);
        _context.TrainingSessions.Add(session);
        await _context.SaveChangesAsync();

        var training = new Training { MemberId = member.Id, MotorcycleId = motorcycle.Id, TrainingSessionId = session.Id, IsCertificateIssued = false };
        await _repository.AddAsync(training);
        training.IsCertificateIssued = true;

        // Act
        await _repository.UpdateAsync(training);

        // Assert
        var updatedTraining = await _repository.GetByIdAsync(training.Id);
        updatedTraining!.IsCertificateIssued.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveTraining()
    {
        // Arrange
        var member = new Member { Name = "John", Surname = "Doe" };
        var motorcycle = new Motorcycle { MemberId = member.Id, BrandName = "Honda", ModelName = "CBR600" };
        var level = new Level { Name = "Beginner" };
        var session = new TrainingSession { LevelId = level.Id, City = "Belgrade", TheoryDate = DateTime.Now };

        _context.Members.Add(member);
        _context.Motorcycles.Add(motorcycle);
        _context.Levels.Add(level);
        _context.TrainingSessions.Add(session);
        await _context.SaveChangesAsync();

        var training = new Training { MemberId = member.Id, MotorcycleId = motorcycle.Id, TrainingSessionId = session.Id };
        await _repository.AddAsync(training);

        // Act
        await _repository.DeleteAsync(training.Id);

        // Assert
        var deletedTraining = await _repository.GetByIdAsync(training.Id);
        deletedTraining.Should().BeNull();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}

