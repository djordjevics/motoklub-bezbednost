using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Xunit;
using FluentAssertions;
using MotoklubBezbednost.Data;
using MotoklubBezbednost.Data.Models;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.API.Tests.Repositories;

public class MemberRepositoryTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly ApplicationDbContext _context;
    private readonly MemberRepository _repository;

    public MemberRepositoryTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection, sqlite =>
                sqlite.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name))
            .Options;

        _context = new ApplicationDbContext(options);
        _context.Database.Migrate();
        _repository = new MemberRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddMemberToDatabase()
    {
        // Arrange
        var member = new MemberDb
        {
            Name = "John",
            Surname = "Doe",
            Email = "john.doe@example.com"
        };

        // Act
        var result = await _repository.AddAsync(member);
        await _context.SaveChangesAsync();

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        var memberInDb = await _context.Members.FindAsync(result.Id);
        memberInDb.Should().NotBeNull();
        memberInDb!.Name.Should().Be("John");
    }

    [Fact]
    public async Task GetByIdAsync_WhenMemberExists_ShouldReturnMember()
    {
        // Arrange
        var member = new MemberDb { Name = "John", Surname = "Doe" };
        await _repository.AddAsync(member);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetByIdAsync(member.Id);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("John");
        result.Surname.Should().Be("Doe");
    }

    [Fact]
    public async Task GetByIdAsync_WhenMemberDoesNotExist_ShouldReturnNull()
    {
        // Act
        var result = await _repository.GetByIdAsync(999);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllMembers()
    {
        // Arrange
        await _repository.AddAsync(new MemberDb { Name = "John", Surname = "Doe" });
        await _repository.AddAsync(new MemberDb { Name = "Jane", Surname = "Smith" });
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateMember()
    {
        // Arrange
        var member = new MemberDb { Name = "John", Surname = "Doe" };
        await _repository.AddAsync(member);
        await _context.SaveChangesAsync();
        member.Surname = "Doe Updated";

        // Act
        await _repository.UpdateAsync(member);
        await _context.SaveChangesAsync();

        // Assert
        var updatedMember = await _repository.GetByIdAsync(member.Id);
        updatedMember!.Surname.Should().Be("Doe Updated");
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveMember()
    {
        // Arrange
        var member = new MemberDb { Name = "John", Surname = "Doe" };
        await _repository.AddAsync(member);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(member.Id);
        await _context.SaveChangesAsync();

        // Assert
        var deletedMember = await _repository.GetByIdAsync(member.Id);
        deletedMember.Should().BeNull();
    }

    [Fact]
    public async Task SearchAsync_ShouldReturnMatchingMembers()
    {
        // Arrange
        await _repository.AddAsync(new MemberDb { Name = "John", Surname = "Doe", Email = "john@example.com" });
        await _repository.AddAsync(new MemberDb { Name = "Jane", Surname = "Smith", Email = "jane@example.com" });
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.SearchAsync("John");

        // Assert
        result.Should().HaveCount(1);
        result.First().Name.Should().Be("John");
    }

    [Fact]
    public async Task SearchAsync_ShouldSearchByEmail()
    {
        // Arrange
        await _repository.AddAsync(new MemberDb { Name = "John", Surname = "Doe", Email = "john@example.com" });
        await _repository.AddAsync(new MemberDb { Name = "Jane", Surname = "Smith", Email = "jane@example.com" });
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.SearchAsync("john@example.com");

        // Assert
        result.Should().HaveCount(1);
        result.First().Email.Should().Be("john@example.com");
    }

    [Fact]
    public async Task SearchAsync_ShouldFindByMemberTypeIdAndActiveTag_WithFlexibleHyphenSpacing()
    {
        var today = DateTime.Today;
        var member = new MemberDb { Name = "Z", Surname = "Test", MemberTypeId = 2 };
        await _repository.AddAsync(member);
        await _context.SaveChangesAsync();

        _context.Tags.Add(new TagDb
        {
            MemberId = member.Id,
            TagNumber = 42,
            ValidFrom = today.AddDays(-1),
            ValidTo = today.AddDays(1),
            CreationTimestamp = today,
        });
        await _context.SaveChangesAsync();

        var spaced = await _repository.SearchAsync(" 2 - 42 ");
        var nospace = await _repository.SearchAsync("2-42");

        spaced.Should().ContainSingle(r => r.Id == member.Id);
        nospace.Should().ContainSingle(r => r.Id == member.Id);
    }

    [Fact]
    public async Task SearchAsync_ShouldFindByMemberTypeNameAndActiveTag()
    {
        var today = DateTime.Today;
        var member = new MemberDb { Name = "Z", Surname = "Tagged", MemberTypeId = 2 };
        await _repository.AddAsync(member);
        await _context.SaveChangesAsync();

        _context.Tags.Add(new TagDb
        {
            MemberId = member.Id,
            TagNumber = 10,
            ValidFrom = today,
            ValidTo = today.AddDays(7),
            CreationTimestamp = today,
        });
        await _context.SaveChangesAsync();

        var result = await _repository.SearchAsync("aktiv - 10");

        result.Should().ContainSingle(r => r.Id == member.Id);
    }

    [Fact]
    public async Task SearchAsync_ByHyphen_ShouldExcludeInactiveOrUndatedTagValidity()
    {
        var today = DateTime.Today;
        var withExpiredTag = new MemberDb { Name = "Exp", Surname = "Old", MemberTypeId = 2 };
        var withUndatedTag = new MemberDb { Name = "Nil", Surname = "Dates", MemberTypeId = 2 };
        await _repository.AddAsync(withExpiredTag);
        await _repository.AddAsync(withUndatedTag);
        await _context.SaveChangesAsync();

        _context.Tags.Add(new TagDb
        {
            MemberId = withExpiredTag.Id,
            TagNumber = 7,
            ValidFrom = today.AddYears(-2),
            ValidTo = today.AddDays(-1),
            CreationTimestamp = today,
        });
        _context.Tags.Add(new TagDb
        {
            MemberId = withUndatedTag.Id,
            TagNumber = 8,
            ValidFrom = null,
            ValidTo = null,
            CreationTimestamp = today,
        });
        await _context.SaveChangesAsync();

        var expiredResults = await _repository.SearchAsync("2-7");
        var undatedResults = await _repository.SearchAsync("2-8");

        expiredResults.Should().BeEmpty();
        undatedResults.Should().BeEmpty();
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }
}

