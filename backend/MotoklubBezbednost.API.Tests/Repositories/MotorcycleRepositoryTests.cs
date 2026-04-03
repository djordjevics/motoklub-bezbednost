using Microsoft.EntityFrameworkCore;
using Xunit;
using FluentAssertions;
using MotoklubBezbednost.Data;
using MotoklubBezbednost.Data.Models;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.API.Tests.Repositories;

public class MotorcycleRepositoryTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly MotorcycleRepository _repository;

    public MotorcycleRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
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
            Member = member,
            BrandName = "Honda",
            ModelName = "CBR600",
            EngineDisplacment = 600
        };

        // Act
        var result = await _repository.AddAsync(motorcycle);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        var motorcycleInDb = await _context.Motorcycles.FindAsync(result.Id);
        motorcycleInDb.Should().NotBeNull();
        motorcycleInDb!.BrandName.Should().Be("Honda");
    }

    [Fact]
    public async Task GetByIdAsync_WhenMotorcycleExists_ShouldReturnMotorcycleWithMember()
    {
        // Arrange
        var member = new MemberDb { Name = "John", Surname = "Doe" };
        _context.Members.Add(member);
        await _context.SaveChangesAsync();

        var motorcycle = new MotorcycleDb { Member = member, BrandName = "Honda", ModelName = "CBR600" };
        await _repository.AddAsync(motorcycle);

        // Act
        var result = await _repository.GetByIdAsync(motorcycle.Id);

        // Assert
        result.Should().NotBeNull();
        result!.BrandName.Should().Be("Honda");
        result.Member.Should().NotBeNull();
        result.Member.Name.Should().Be("John");
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

        await _repository.AddAsync(new MotorcycleDb { Member = member, BrandName = "Honda", ModelName = "CBR600" });
        await _repository.AddAsync(new MotorcycleDb { Member = member, BrandName = "Yamaha", ModelName = "R1" });

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetAllWithMemberAsync_ShouldReturnMotorcyclesWithMembers()
    {
        // Arrange
        var member = new MemberDb { Name = "John", Surname = "Doe" };
        _context.Members.Add(member);
        await _context.SaveChangesAsync();

        await _repository.AddAsync(new MotorcycleDb { Member = member, BrandName = "Honda", ModelName = "CBR600" });

        // Act
        var result = await _repository.GetAllWithMemberAsync();

        // Assert
        result.Should().HaveCount(1);
        result.First().Member.Should().NotBeNull();
        result.First().Member.Name.Should().Be("John");
    }

    [Fact]
    public async Task GetByMemberIdAsync_ShouldReturnMemberMotorcycles()
    {
        // Arrange
        var member1 = new MemberDb { Name = "John", Surname = "Doe" };
        var member2 = new MemberDb { Name = "Jane", Surname = "Smith" };
        _context.Members.AddRange(member1, member2);
        await _context.SaveChangesAsync();

        await _repository.AddAsync(new MotorcycleDb { Member = member1, BrandName = "Honda", ModelName = "CBR600" });
        await _repository.AddAsync(new MotorcycleDb { Member = member1, BrandName = "Yamaha", ModelName = "R1" });
        await _repository.AddAsync(new MotorcycleDb { Member = member2, BrandName = "Kawasaki", ModelName = "Ninja" });

        // Act
        var result = await _repository.GetByMemberIdAsync(member1.Id);

        // Assert
        result.Should().HaveCount(2);
        result.All(m => m.Member.Id == member1.Id).Should().BeTrue();
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateMotorcycle()
    {
        // Arrange
        var member = new MemberDb { Name = "John", Surname = "Doe" };
        _context.Members.Add(member);
        await _context.SaveChangesAsync();

        var motorcycle = new MotorcycleDb { Member = member, BrandName = "Honda", ModelName = "CBR600" };
        await _repository.AddAsync(motorcycle);
        motorcycle.ModelName = "CBR600 Updated";

        // Act
        await _repository.UpdateAsync(motorcycle);

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

        var motorcycle = new MotorcycleDb { Member = member, BrandName = "Honda", ModelName = "CBR600" };
        await _repository.AddAsync(motorcycle);

        // Act
        await _repository.DeleteAsync(motorcycle.Id);

        // Assert
        var deletedMotorcycle = await _repository.GetByIdAsync(motorcycle.Id);
        deletedMotorcycle.Should().BeNull();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}

