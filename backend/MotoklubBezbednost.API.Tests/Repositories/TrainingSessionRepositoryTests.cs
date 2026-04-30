using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Xunit;
using FluentAssertions;
using MotoklubBezbednost.Data;
using MotoklubBezbednost.Data.Models;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.API.Tests.Repositories;

public class TrainingSessionRepositoryTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly ApplicationDbContext _context;
    private readonly TrainingSessionRepository _repository;

    public TrainingSessionRepositoryTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection, sqlite =>
                sqlite.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name))
            .Options;

        _context = new ApplicationDbContext(options);
        _context.Database.Migrate();
        _repository = new TrainingSessionRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddTrainingSessionToDatabase()
    {
        // Arrange
        var level = new LevelDb { Name = "Beginner" };
        _context.Levels.Add(level);
        await _context.SaveChangesAsync();

        var session = new TrainingSessionDb
        {
            Level = level,
            City = "Belgrade",
            TheoryDate = DateTime.Now,
            Price = 5000
        };

        // Act
        var result = await _repository.AddAsync(session);
        await _context.SaveChangesAsync();

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        var sessionInDb = await _context.TrainingSessions.FindAsync(result.Id);
        sessionInDb.Should().NotBeNull();
        sessionInDb!.City.Should().Be("Belgrade");
    }

    [Fact]
    public async Task GetByIdAsync_WhenSessionExists_ShouldReturnSession()
    {
        // Arrange
        var level = new LevelDb { Name = "Beginner" };
        _context.Levels.Add(level);
        await _context.SaveChangesAsync();

        var session = new TrainingSessionDb { Level = level, City = "Belgrade", TheoryDate = DateTime.Now };
        await _repository.AddAsync(session);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(session.Id);

        // Assert
        result.Should().NotBeNull();
        result!.City.Should().Be("Belgrade");
    }

    [Fact]
    public async Task GetByIdWithDetailsAsync_ShouldReturnSessionWithDetails()
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
        _context.Trainings.Add(training);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdWithDetailsAsync(session.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Level.Should().NotBeNull();
        result.Trainings.Should().NotBeEmpty();
        result.Trainings.First().Motorcycle.Should().NotBeNull();
    }

    [Fact]
    public async Task GetAllWithDetailsAsync_ShouldReturnSessionsWithDetails()
    {
        // Arrange
        var level = new LevelDb { Name = "Beginner" };
        _context.Levels.Add(level);
        await _context.SaveChangesAsync();

        var session1 = new TrainingSessionDb { Level = level, City = "Belgrade", TheoryDate = DateTime.Now };
        var session2 = new TrainingSessionDb { Level = level, City = "Novi Sad", TheoryDate = DateTime.Now.AddDays(1) };
        await _repository.AddAsync(session1);
        await _repository.AddAsync(session2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllWithDetailsAsync();

        // Assert
        result.Should().HaveCount(2);
        result.All(s => s.Level != null).Should().BeTrue();
    }

    [Fact]
    public async Task GetAllWithDetailsAsync_ShouldOrderByDateDescending()
    {
        // Arrange
        var level = new LevelDb { Name = "Beginner" };
        _context.Levels.Add(level);
        await _context.SaveChangesAsync();

        var session1 = new TrainingSessionDb { Level = level, City = "Belgrade", TheoryDate = DateTime.Now };
        var session2 = new TrainingSessionDb { Level = level, City = "Novi Sad", TheoryDate = DateTime.Now.AddDays(1) };
        await _repository.AddAsync(session1);
        await _repository.AddAsync(session2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllWithDetailsAsync();

        // Assert
        result.Should().HaveCount(2);
        result.First().City.Should().Be("Novi Sad"); // Most recent first
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateTrainingSession()
    {
        // Arrange
        var level = new LevelDb { Name = "Beginner" };
        _context.Levels.Add(level);
        await _context.SaveChangesAsync();

        var session = new TrainingSessionDb { Level = level, City = "Belgrade", TheoryDate = DateTime.Now };
        await _repository.AddAsync(session);
        await _context.SaveChangesAsync();
        session.City = "Novi Sad";

        // Act
        await _repository.UpdateAsync(session);
        await _context.SaveChangesAsync();

        // Assert
        var updatedSession = await _repository.GetByIdAsync(session.Id);
        updatedSession!.City.Should().Be("Novi Sad");
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveTrainingSession()
    {
        // Arrange
        var level = new LevelDb { Name = "Beginner" };
        _context.Levels.Add(level);
        await _context.SaveChangesAsync();

        var session = new TrainingSessionDb { Level = level, City = "Belgrade", TheoryDate = DateTime.Now };
        await _repository.AddAsync(session);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(session.Id);
        await _context.SaveChangesAsync();

        // Assert
        var deletedSession = await _repository.GetByIdAsync(session.Id);
        deletedSession.Should().BeNull();
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }
}

