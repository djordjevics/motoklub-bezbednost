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
        var motorcycle = new Motorcycle { Member = member, BrandName = "Honda", ModelName = "CBR600" };
        var level = new Level { Name = "Beginner" };
        var session = new TrainingSession { Level = level, City = "Belgrade", TheoryDate = DateTime.Now };

        _context.Members.Add(member);
        _context.Motorcycles.Add(motorcycle);
        _context.Levels.Add(level);
        _context.TrainingSessions.Add(session);
        await _context.SaveChangesAsync();

        var training = new Training
        {
            Member = member,
            Motorcycle = motorcycle,
            TrainingSession = session,
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
        var motorcycle = new Motorcycle { Member = member, BrandName = "Honda", ModelName = "CBR600" };
        var level = new Level { Name = "Beginner" };
        var session = new TrainingSession { Level = level, City = "Belgrade", TheoryDate = DateTime.Now };

        _context.Members.Add(member);
        _context.Motorcycles.Add(motorcycle);
        _context.Levels.Add(level);
        _context.TrainingSessions.Add(session);
        await _context.SaveChangesAsync();

        var training = new Training { Member = member, Motorcycle = motorcycle, TrainingSession = session };
        await _repository.AddAsync(training);

        // Act
        var result = await _repository.GetByIdAsync(training.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Member.Id.Should().Be(member.Id);
    }

    [Fact]
    public async Task GetByIdWithDetailsAsync_ShouldReturnTrainingWithDetails()
    {
        // Arrange
        var member = new Member { Name = "John", Surname = "Doe" };
        var motorcycle = new Motorcycle { Member = member, BrandName = "Honda", ModelName = "CBR600" };
        var level = new Level { Name = "Beginner" };
        var session = new TrainingSession { Level = level, City = "Belgrade", TheoryDate = DateTime.Now };

        _context.Members.Add(member);
        _context.Motorcycles.Add(motorcycle);
        _context.Levels.Add(level);
        _context.TrainingSessions.Add(session);
        await _context.SaveChangesAsync();

        var training = new Training { Member = member, Motorcycle = motorcycle, TrainingSession = session };
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
        var motorcycle1 = new Motorcycle { Member = member1, BrandName = "Honda", ModelName = "CBR600" };
        var motorcycle2 = new Motorcycle { Member = member2, BrandName = "Yamaha", ModelName = "R1" };
        var level = new Level { Name = "Beginner" };
        var session = new TrainingSession { Level = level, City = "Belgrade", TheoryDate = DateTime.Now };

        _context.Members.AddRange(member1, member2);
        _context.Motorcycles.AddRange(motorcycle1, motorcycle2);
        _context.Levels.Add(level);
        _context.TrainingSessions.Add(session);
        await _context.SaveChangesAsync();

        await _repository.AddAsync(new Training { Member = member1, Motorcycle = motorcycle1, TrainingSession = session });
        await _repository.AddAsync(new Training { Member = member1, Motorcycle = motorcycle1, TrainingSession = session });
        await _repository.AddAsync(new Training { Member = member2, Motorcycle = motorcycle2, TrainingSession = session });

        // Act
        var result = await _repository.GetByMemberIdAsync(member1.Id);

        // Assert
        result.Should().HaveCount(2);
        result.All(t => t.Member.Id == member1.Id).Should().BeTrue();
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateTraining()
    {
        // Arrange
        var member = new Member { Name = "John", Surname = "Doe" };
        var motorcycle = new Motorcycle { Member = member, BrandName = "Honda", ModelName = "CBR600" };
        var level = new Level { Name = "Beginner" };
        var session = new TrainingSession { Level = level, City = "Belgrade", TheoryDate = DateTime.Now };

        _context.Members.Add(member);
        _context.Motorcycles.Add(motorcycle);
        _context.Levels.Add(level);
        _context.TrainingSessions.Add(session);
        await _context.SaveChangesAsync();

        var training = new Training { Member = member, Motorcycle = motorcycle, TrainingSession = session, IsCertificateIssued = false };
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
        var motorcycle = new Motorcycle { Member = member, BrandName = "Honda", ModelName = "CBR600" };
        var level = new Level { Name = "Beginner" };
        var session = new TrainingSession { Level = level, City = "Belgrade", TheoryDate = DateTime.Now };

        _context.Members.Add(member);
        _context.Motorcycles.Add(motorcycle);
        _context.Levels.Add(level);
        _context.TrainingSessions.Add(session);
        await _context.SaveChangesAsync();

        var training = new Training { Member = member, Motorcycle = motorcycle, TrainingSession = session };
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

