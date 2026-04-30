using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Xunit;
using FluentAssertions;
using MotoklubBezbednost.Data;
using MotoklubBezbednost.Data.Models;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.API.Tests.Repositories;

public class TrainingRepositoryTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly ApplicationDbContext _context;
    private readonly TrainingRepository _repository;

    public TrainingRepositoryTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection, sqlite =>
                sqlite.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name))
            .Options;

        _context = new ApplicationDbContext(options);
        _context.Database.Migrate();
        _repository = new TrainingRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddTrainingToDatabase()
    {
        // Arrange
        var member = new MemberDb { Name = "John", Surname = "Doe" };
        _context.Members.Add(member);
        await _context.SaveChangesAsync();

        var motorcycle = new MotorcycleDb { MemberId = member.Id, BrandName = "Honda", ModelName = "CBR600" };
        var level = new LevelDb { Name = "Beginner" };
        var session = new TrainingSessionDb { Level = level, City = "Belgrade", TheoryDate = DateTime.Now };

        _context.Motorcycles.Add(motorcycle);
        _context.Levels.Add(level);
        _context.TrainingSessions.Add(session);
        await _context.SaveChangesAsync();

        var training = new TrainingDb
        {
            MemberId = member.Id,
            Motorcycle = motorcycle,
            TrainingSession = session,
            IsCertificateIssued = false
        };

        // Act
        var result = await _repository.AddAsync(training);
        await _context.SaveChangesAsync();

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
        var member = new MemberDb { Name = "John", Surname = "Doe" };
        _context.Members.Add(member);
        await _context.SaveChangesAsync();

        var motorcycle = new MotorcycleDb { MemberId = member.Id, BrandName = "Honda", ModelName = "CBR600" };
        var level = new LevelDb { Name = "Beginner" };
        var session = new TrainingSessionDb { Level = level, City = "Belgrade", TheoryDate = DateTime.Now };

        _context.Motorcycles.Add(motorcycle);
        _context.Levels.Add(level);
        _context.TrainingSessions.Add(session);
        await _context.SaveChangesAsync();

        var training = new TrainingDb { MemberId = member.Id, Motorcycle = motorcycle, TrainingSession = session };
        await _repository.AddAsync(training);
        await _context.SaveChangesAsync();

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
        var member = new MemberDb { Name = "John", Surname = "Doe" };
        _context.Members.Add(member);
        await _context.SaveChangesAsync();

        var motorcycle = new MotorcycleDb { MemberId = member.Id, BrandName = "Honda", ModelName = "CBR600" };
        var level = new LevelDb { Name = "Beginner" };
        var session = new TrainingSessionDb { Level = level, City = "Belgrade", TheoryDate = DateTime.Now };

        _context.Motorcycles.Add(motorcycle);
        _context.Levels.Add(level);
        _context.TrainingSessions.Add(session);
        await _context.SaveChangesAsync();

        var training = new TrainingDb { MemberId = member.Id, Motorcycle = motorcycle, TrainingSession = session };
        await _repository.AddAsync(training);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdWithDetailsAsync(training.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Motorcycle.Should().NotBeNull();
        result.TrainingSession.Should().NotBeNull();
        result.TrainingSession!.Level.Should().NotBeNull();
    }

    [Fact]
    public async Task GetByMemberIdAsync_ShouldReturnMemberTrainings()
    {
        // Arrange
        var member1 = new MemberDb { Name = "John", Surname = "Doe" };
        var member2 = new MemberDb { Name = "Jane", Surname = "Smith" };
        _context.Members.AddRange(member1, member2);
        await _context.SaveChangesAsync();

        var motorcycle1 = new MotorcycleDb { MemberId = member1.Id, BrandName = "Honda", ModelName = "CBR600" };
        var motorcycle2 = new MotorcycleDb { MemberId = member2.Id, BrandName = "Yamaha", ModelName = "R1" };
        var level = new LevelDb { Name = "Beginner" };
        var session = new TrainingSessionDb { Level = level, City = "Belgrade", TheoryDate = DateTime.Now };

        _context.Motorcycles.AddRange(motorcycle1, motorcycle2);
        _context.Levels.Add(level);
        _context.TrainingSessions.Add(session);
        await _context.SaveChangesAsync();

        await _repository.AddAsync(new TrainingDb { MemberId = member1.Id, Motorcycle = motorcycle1, TrainingSession = session });
        await _repository.AddAsync(new TrainingDb { MemberId = member1.Id, Motorcycle = motorcycle1, TrainingSession = session });
        await _repository.AddAsync(new TrainingDb { MemberId = member2.Id, Motorcycle = motorcycle2, TrainingSession = session });
        await _context.SaveChangesAsync();

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
        var member = new MemberDb { Name = "John", Surname = "Doe" };
        _context.Members.Add(member);
        await _context.SaveChangesAsync();

        var motorcycle = new MotorcycleDb { MemberId = member.Id, BrandName = "Honda", ModelName = "CBR600" };
        var level = new LevelDb { Name = "Beginner" };
        var session = new TrainingSessionDb { Level = level, City = "Belgrade", TheoryDate = DateTime.Now };

        _context.Motorcycles.Add(motorcycle);
        _context.Levels.Add(level);
        _context.TrainingSessions.Add(session);
        await _context.SaveChangesAsync();

        var training = new TrainingDb { MemberId = member.Id, Motorcycle = motorcycle, TrainingSession = session, IsCertificateIssued = false };
        await _repository.AddAsync(training);
        await _context.SaveChangesAsync();
        training.IsCertificateIssued = true;

        // Act
        await _repository.UpdateAsync(training);
        await _context.SaveChangesAsync();

        // Assert
        var updatedTraining = await _repository.GetByIdAsync(training.Id);
        updatedTraining!.IsCertificateIssued.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveTraining()
    {
        // Arrange
        var member = new MemberDb { Name = "John", Surname = "Doe" };
        _context.Members.Add(member);
        await _context.SaveChangesAsync();

        var motorcycle = new MotorcycleDb { MemberId = member.Id, BrandName = "Honda", ModelName = "CBR600" };
        var level = new LevelDb { Name = "Beginner" };
        var session = new TrainingSessionDb { Level = level, City = "Belgrade", TheoryDate = DateTime.Now };

        _context.Motorcycles.Add(motorcycle);
        _context.Levels.Add(level);
        _context.TrainingSessions.Add(session);
        await _context.SaveChangesAsync();

        var training = new TrainingDb { MemberId = member.Id, Motorcycle = motorcycle, TrainingSession = session };
        await _repository.AddAsync(training);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(training.Id);
        await _context.SaveChangesAsync();

        // Assert
        var deletedTraining = await _repository.GetByIdAsync(training.Id);
        deletedTraining.Should().BeNull();
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }
}

