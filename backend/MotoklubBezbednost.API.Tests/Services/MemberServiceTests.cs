using Xunit;
using Moq;
using FluentAssertions;
using MotoklubBezbednost.API.Models;
using MotoklubBezbednost.API.Repositories;
using MotoklubBezbednost.API.Services;

namespace MotoklubBezbednost.API.Tests.Services;

public class MemberServiceTests
{
    private readonly Mock<IMemberRepository> _mockRepository;
    private readonly MemberService _memberService;

    public MemberServiceTests()
    {
        _mockRepository = new Mock<IMemberRepository>();
        _memberService = new MemberService(_mockRepository.Object);
    }

    [Fact]
    public async Task GetAllMembersAsync_ShouldReturnAllMembers()
    {
        // Arrange
        var expectedMembers = new List<Member>
        {
            new Member { Id = 1, Name = "John", Surname = "Doe" },
            new Member { Id = 2, Name = "Jane", Surname = "Smith" }
        };

        _mockRepository.Setup(r => r.GetAllWithDetailsAsync())
            .ReturnsAsync(expectedMembers);

        // Act
        var result = await _memberService.GetAllMembersAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(expectedMembers);
        _mockRepository.Verify(r => r.GetAllWithDetailsAsync(), Times.Once);
    }

    [Fact]
    public async Task GetMemberByIdAsync_WhenMemberExists_ShouldReturnMember()
    {
        // Arrange
        var memberId = 1;
        var expectedMember = new Member { Id = memberId, Name = "John", Surname = "Doe" };

        _mockRepository.Setup(r => r.GetByIdWithDetailsAsync(memberId))
            .ReturnsAsync(expectedMember);

        // Act
        var result = await _memberService.GetMemberByIdAsync(memberId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedMember);
        _mockRepository.Verify(r => r.GetByIdWithDetailsAsync(memberId), Times.Once);
    }

    [Fact]
    public async Task GetMemberByIdAsync_WhenMemberDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var memberId = 999;
        _mockRepository.Setup(r => r.GetByIdWithDetailsAsync(memberId))
            .ReturnsAsync((Member?)null);

        // Act
        var result = await _memberService.GetMemberByIdAsync(memberId);

        // Assert
        result.Should().BeNull();
        _mockRepository.Verify(r => r.GetByIdWithDetailsAsync(memberId), Times.Once);
    }

    [Fact]
    public async Task CreateMemberAsync_ShouldAddAndReturnMember()
    {
        // Arrange
        var newMember = new Member { Name = "John", Surname = "Doe" };
        var createdMember = new Member { Id = 1, Name = "John", Surname = "Doe" };

        _mockRepository.Setup(r => r.AddAsync(newMember))
            .ReturnsAsync(createdMember);

        // Act
        var result = await _memberService.CreateMemberAsync(newMember);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Name.Should().Be("John");
        _mockRepository.Verify(r => r.AddAsync(newMember), Times.Once);
    }

    [Fact]
    public async Task UpdateMemberAsync_ShouldUpdateMember()
    {
        // Arrange
        var member = new Member { Id = 1, Name = "John", Surname = "Doe Updated" };

        _mockRepository.Setup(r => r.UpdateAsync(member))
            .Returns(Task.CompletedTask);

        // Act
        await _memberService.UpdateMemberAsync(member);

        // Assert
        _mockRepository.Verify(r => r.UpdateAsync(member), Times.Once);
    }

    [Fact]
    public async Task DeleteMemberAsync_ShouldDeleteMember()
    {
        // Arrange
        var memberId = 1;
        _mockRepository.Setup(r => r.DeleteAsync(memberId))
            .Returns(Task.CompletedTask);

        // Act
        await _memberService.DeleteMemberAsync(memberId);

        // Assert
        _mockRepository.Verify(r => r.DeleteAsync(memberId), Times.Once);
    }

    [Fact]
    public async Task SearchMembersAsync_ShouldReturnMatchingMembers()
    {
        // Arrange
        var searchQuery = "John";
        var expectedMembers = new List<Member>
        {
            new Member { Id = 1, Name = "John", Surname = "Doe" }
        };

        _mockRepository.Setup(r => r.SearchAsync(searchQuery))
            .ReturnsAsync(expectedMembers);

        // Act
        var result = await _memberService.SearchMembersAsync(searchQuery);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.Should().BeEquivalentTo(expectedMembers);
        _mockRepository.Verify(r => r.SearchAsync(searchQuery), Times.Once);
    }
}

