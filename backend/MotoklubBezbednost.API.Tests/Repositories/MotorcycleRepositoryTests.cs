using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Xunit;
using FluentAssertions;
using MotoklubBezbednost.Data;
using MotoklubBezbednost.Data.Models;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.API.Tests.Repositories;

public class MotorcycleRepositoryTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly ApplicationDbContext _context;
    private readonly MotorcycleRepository _repository;

    public MotorcycleRepositoryTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection, sqlite =>
                sqlite.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name))
            .Options;

        _context = new ApplicationDbContext(options);
        _context.Database.Migrate();
        _repository = new MotorcycleRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddMotorcycleToDatabase()
    {
        // Arrange
        var member = new MemberDb { Name = "John", Surname = "Doe" };
        _context.Members.Add(member);
        await _context.SaveChangesAsync();

        var motorcycle = new MotorcycleDb
        {
            MemberId = member.Id,
            BrandName = "Honda",
            ModelName = "CBR600",
            EngineDisplacment = 600
        };

        // Act
        var result = await _repository.AddAsync(motorcycle);
        await _context.SaveChangesAsync();

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        var motorcycleInDb = await _context.Motorcycles.FindAsync(result.Id);
        motorcycleInDb.Should().NotBeNull();
        motorcycleInDb!.BrandName.Should().Be("Honda");
    }

    [Fact]
    public async Task GetByIdAsync_WhenMotorcycleExists_ShouldReturnMotorcycle()
    {
        // Arrange
        var member = new MemberDb { Name = "John", Surname = "Doe" };
        _context.Members.Add(member);
        await _context.SaveChangesAsync();

        var motorcycle = new MotorcycleDb { MemberId = member.Id, BrandName = "Honda", ModelName = "CBR600" };
        await _repository.AddAsync(motorcycle);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(motorcycle.Id);

        // Assert
        result.Should().NotBeNull();
        result!.BrandName.Should().Be("Honda");
    }

    [Fact]
    public async Task GetByIdAsync_WhenMotorcycleDoesNotExist_ShouldReturnNull()
    {
        // Act
        var result = await _repository.GetByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllMotorcycles()
    {
        // Arrange
        var member = new MemberDb { Name = "John", Surname = "Doe" };
        _context.Members.Add(member);
        await _context.SaveChangesAsync();

        await _repository.AddAsync(new MotorcycleDb { MemberId = member.Id, BrandName = "Honda", ModelName = "CBR600" });
        await _repository.AddAsync(new MotorcycleDb { MemberId = member.Id, BrandName = "Yamaha", ModelName = "R1" });
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetByMemberIdAsync_ShouldReturnMemberMotorcycles()
    {
        // Arrange
        var member1 = new MemberDb { Name = "John", Surname = "Doe" };
        var member2 = new MemberDb { Name = "Jane", Surname = "Smith" };
        _context.Members.AddRange(member1, member2);
        await _context.SaveChangesAsync();

        await _repository.AddAsync(new MotorcycleDb { MemberId = member1.Id, BrandName = "Honda", ModelName = "CBR600" });
        await _repository.AddAsync(new MotorcycleDb { MemberId = member1.Id, BrandName = "Yamaha", ModelName = "R1" });
        await _repository.AddAsync(new MotorcycleDb { MemberId = member2.Id, BrandName = "Kawasaki", ModelName = "Ninja" });
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByMemberIdAsync(member1.Id);

        // Assert
        result.Should().HaveCount(2);
        result.All(m => m.MemberId == member1.Id).Should().BeTrue();
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateMotorcycle()
    {
        // Arrange
        var member = new MemberDb { Name = "John", Surname = "Doe" };
        _context.Members.Add(member);
        await _context.SaveChangesAsync();

        var motorcycle = new MotorcycleDb { MemberId = member.Id, BrandName = "Honda", ModelName = "CBR600" };
        await _repository.AddAsync(motorcycle);
        await _context.SaveChangesAsync();
        motorcycle.ModelName = "CBR600 Updated";

        // Act
        await _repository.UpdateAsync(motorcycle);
        await _context.SaveChangesAsync();

        // Assert
        var updatedMotorcycle = await _repository.GetByIdAsync(motorcycle.Id);
        updatedMotorcycle!.ModelName.Should().Be("CBR600 Updated");
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveMotorcycle()
    {
        // Arrange
        var member = new MemberDb { Name = "John", Surname = "Doe" };
        _context.Members.Add(member);
        await _context.SaveChangesAsync();

        var motorcycle = new MotorcycleDb { MemberId = member.Id, BrandName = "Honda", ModelName = "CBR600" };
        await _repository.AddAsync(motorcycle);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(motorcycle.Id);
        await _context.SaveChangesAsync();

        // Assert
        var deletedMotorcycle = await _repository.GetByIdAsync(motorcycle.Id);
        deletedMotorcycle.Should().BeNull();
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }
}

