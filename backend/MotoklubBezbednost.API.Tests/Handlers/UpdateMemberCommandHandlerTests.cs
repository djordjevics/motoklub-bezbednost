using AutoMapper;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MotoklubBezbednost.API.Mappings;
using MotoklubBezbednost.Business.Cqrs.Members.Commands;
using MotoklubBezbednost.Business.Cqrs.Members.Handlers;
using MotoklubBezbednost.Business.Mappings;
using MotoklubBezbednost.Data;
using MotoklubBezbednost.Data.Exceptions;
using MotoklubBezbednost.Data.Models;
using MotoklubBezbednost.Data.Repositories;
using MotoklubBezbednost.Data.UnitOfWork;
using Xunit;

namespace MotoklubBezbednost.API.Tests.Handlers;

public sealed class UpdateMemberCommandHandlerTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly ApplicationDbContext _context;

    public UpdateMemberCommandHandlerTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection, sqlite =>
                sqlite.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name))
            .Options;

        _context = new ApplicationDbContext(options);
        _context.Database.Migrate();
    }

    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }

    private static IMapper CreateMapper()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAutoMapper(_ => { }, typeof(BusinessMappingProfile).Assembly, typeof(ApiMappingProfile).Assembly);
        return services.BuildServiceProvider().GetRequiredService<IMapper>();
    }

    [Fact]
    public async Task Handle_Active_member_Persists_MembershipExemptManual_To_Database()
    {
        await _context.Members.AddAsync(new MemberDb { Name = "Jane", Surname = "Doe", MembershipExemptManual = false });
        await _context.SaveChangesAsync();

        var id = (await _context.Members.SingleAsync()).Id;

        var handler = new UpdateMemberCommandHandler(
            new MemberRepository(_context),
            new UnitOfWork(_context, new SqliteDataExceptionTranslator()),
            CreateMapper());

        await handler.Handle(new UpdateMemberCommand { Id = id, MembershipExemptManual = true }, CancellationToken.None);

        _context.ChangeTracker.Clear();
        var row = await _context.Members.AsNoTracking().SingleAsync(m => m.Id == id);
        row.MembershipExemptManual.Should().BeTrue();
    }
}
