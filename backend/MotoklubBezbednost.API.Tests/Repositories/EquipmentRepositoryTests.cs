using Microsoft.EntityFrameworkCore;
using Xunit;
using FluentAssertions;
using MotoklubBezbednost.API.Data;
using MotoklubBezbednost.API.Models;
using MotoklubBezbednost.API.Repositories;

namespace MotoklubBezbednost.API.Tests.Repositories;

public class EquipmentRepositoryTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly EquipmentRepository _repository;

    public EquipmentRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new EquipmentRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddEquipmentToDatabase()
    {
        // Arrange
        var member = new Member { Name = "John", Surname = "Doe" };
        _context.Members.Add(member);
        await _context.SaveChangesAsync();

        var equipment = new Equipment
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

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        var equipmentInDb = await _context.Equipment.FindAsync(result.Id);
        equipmentInDb.Should().NotBeNull();
        equipmentInDb!.Pants.Should().BeTrue();
        equipmentInDb.Jacket.Should().BeTrue();
    }

    [Fact]
    public async Task GetByIdAsync_WhenEquipmentExists_ShouldReturnEquipmentWithMember()
    {
        // Arrange
        var member = new Member { Name = "John", Surname = "Doe" };
        _context.Members.Add(member);
        await _context.SaveChangesAsync();

        var equipment = new Equipment { MemberId = member.Id, Pants = true, Jacket = true };
        await _repository.AddAsync(equipment);

        // Act
        var result = await _repository.GetByIdAsync(equipment.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Pants.Should().BeTrue();
        result.Member.Should().NotBeNull();
        result.Member.Name.Should().Be("John");
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
        var member1 = new Member { Name = "John", Surname = "Doe" };
        var member2 = new Member { Name = "Jane", Surname = "Smith" };
        _context.Members.AddRange(member1, member2);
        await _context.SaveChangesAsync();

        await _repository.AddAsync(new Equipment { MemberId = member1.Id, Pants = true, Jacket = true });
        await _repository.AddAsync(new Equipment { MemberId = member2.Id, Pants = true, Jacket = false });

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetAllWithMemberAsync_ShouldReturnEquipmentWithMembers()
    {
        // Arrange
        var member = new Member { Name = "John", Surname = "Doe" };
        _context.Members.Add(member);
        await _context.SaveChangesAsync();

        await _repository.AddAsync(new Equipment { MemberId = member.Id, Pants = true, Jacket = true });

        // Act
        var result = await _repository.GetAllWithMemberAsync();

        // Assert
        result.Should().HaveCount(1);
        result.First().Member.Should().NotBeNull();
        result.First().Member.Name.Should().Be("John");
    }

    [Fact]
    public async Task GetByMemberIdAsync_ShouldReturnMemberEquipment()
    {
        // Arrange
        var member = new Member { Name = "John", Surname = "Doe" };
        _context.Members.Add(member);
        await _context.SaveChangesAsync();

        var equipment = new Equipment { MemberId = member.Id, Pants = true, Jacket = true };
        await _repository.AddAsync(equipment);

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
        var member = new Member { Name = "John", Surname = "Doe" };
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
        var member = new Member { Name = "John", Surname = "Doe" };
        _context.Members.Add(member);
        await _context.SaveChangesAsync();

        var equipment = new Equipment { MemberId = member.Id, Pants = true, Jacket = true };
        await _repository.AddAsync(equipment);
        equipment.Pants = false;

        // Act
        await _repository.UpdateAsync(equipment);

        // Assert
        var updatedEquipment = await _repository.GetByIdAsync(equipment.Id);
        updatedEquipment!.Pants.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveEquipment()
    {
        // Arrange
        var member = new Member { Name = "John", Surname = "Doe" };
        _context.Members.Add(member);
        await _context.SaveChangesAsync();

        var equipment = new Equipment { MemberId = member.Id, Pants = true, Jacket = true };
        await _repository.AddAsync(equipment);

        // Act
        await _repository.DeleteAsync(equipment.Id);

        // Assert
        var deletedEquipment = await _repository.GetByIdAsync(equipment.Id);
        deletedEquipment.Should().BeNull();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}

