using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Xunit;
using FluentAssertions;
using MotoklubBezbednost.Data;
using MotoklubBezbednost.Data.Models;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.API.Tests.Repositories;

public class EquipmentRepositoryTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly ApplicationDbContext _context;
    private readonly EquipmentRepository _repository;

    public EquipmentRepositoryTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection, sqlite =>
                sqlite.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name))
            .Options;

        _context = new ApplicationDbContext(options);
        _context.Database.Migrate();
        _repository = new EquipmentRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddEquipmentToDatabase()
    {
        // Arrange
        var member = new MemberDb { Name = "John", Surname = "Doe" };
        _context.Members.Add(member);
        await _context.SaveChangesAsync();

        var equipment = new EquipmentDb
        {
            MemberId = member.Id,
            Pants = true,
            Jacket = true,
            Vest = false,
            WorkShirt = true,
            FormalShirt = false
        };

        // Act
        var result = await _repository.AddAsync(equipment);
        await _context.SaveChangesAsync();

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        var equipmentInDb = await _context.Equipment.FindAsync(result.Id);
        equipmentInDb.Should().NotBeNull();
        equipmentInDb!.Pants.Should().BeTrue();
        equipmentInDb.Jacket.Should().BeTrue();
    }

    [Fact]
    public async Task GetByIdAsync_WhenEquipmentExists_ShouldReturnEquipment()
    {
        // Arrange
        var member = new MemberDb { Name = "John", Surname = "Doe" };
        _context.Members.Add(member);
        await _context.SaveChangesAsync();

        var equipment = new EquipmentDb { MemberId = member.Id, Pants = true, Jacket = true };
        await _repository.AddAsync(equipment);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(equipment.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Pants.Should().BeTrue();
    }

    [Fact]
    public async Task GetByIdAsync_WhenEquipmentDoesNotExist_ShouldReturnNull()
    {
        // Act
        var result = await _repository.GetByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllEquipment()
    {
        // Arrange
        var member1 = new MemberDb { Name = "John", Surname = "Doe" };
        var member2 = new MemberDb { Name = "Jane", Surname = "Smith" };
        _context.Members.AddRange(member1, member2);
        await _context.SaveChangesAsync();

        await _repository.AddAsync(new EquipmentDb { MemberId = member1.Id, Pants = true, Jacket = true });
        await _repository.AddAsync(new EquipmentDb { MemberId = member2.Id, Pants = true, Jacket = false });
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetByMemberIdAsync_ShouldReturnMemberEquipment()
    {
        // Arrange
        var member = new MemberDb { Name = "John", Surname = "Doe" };
        _context.Members.Add(member);
        await _context.SaveChangesAsync();

        var equipment = new EquipmentDb { MemberId = member.Id, Pants = true, Jacket = true };
        await _repository.AddAsync(equipment);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByMemberIdAsync(member.Id);

        // Assert
        result.Should().NotBeNull();
        result!.MemberId.Should().Be(member.Id);
        result.Pants.Should().BeTrue();
    }

    [Fact]
    public async Task GetByMemberIdAsync_WhenEquipmentDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var member = new MemberDb { Name = "John", Surname = "Doe" };
        _context.Members.Add(member);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByMemberIdAsync(member.Id);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEquipment()
    {
        // Arrange
        var member = new MemberDb { Name = "John", Surname = "Doe" };
        _context.Members.Add(member);
        await _context.SaveChangesAsync();

        var equipment = new EquipmentDb { MemberId = member.Id, Pants = true, Jacket = true };
        await _repository.AddAsync(equipment);
        await _context.SaveChangesAsync();
        equipment.Pants = false;

        // Act
        await _repository.UpdateAsync(equipment);
        await _context.SaveChangesAsync();

        // Assert
        var updatedEquipment = await _repository.GetByIdAsync(equipment.Id);
        updatedEquipment!.Pants.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveEquipment()
    {
        // Arrange
        var member = new MemberDb { Name = "John", Surname = "Doe" };
        _context.Members.Add(member);
        await _context.SaveChangesAsync();

        var equipment = new EquipmentDb { MemberId = member.Id, Pants = true, Jacket = true };
        await _repository.AddAsync(equipment);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(equipment.Id);
        await _context.SaveChangesAsync();

        // Assert
        var deletedEquipment = await _repository.GetByIdAsync(equipment.Id);
        deletedEquipment.Should().BeNull();
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }
}

