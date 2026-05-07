using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MotoklubBezbednost.API.Mappings;
using MotoklubBezbednost.API.Models.Requests;
using MotoklubBezbednost.Business.Cqrs.Members.Commands;
using MotoklubBezbednost.Business.Mappings;
using MotoklubBezbednost.Data.Models;
using Xunit;

namespace MotoklubBezbednost.API.Tests.Mappings;

public sealed class UpdateMemberMappingTests
{
    private static IMapper CreateMapper()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddAutoMapper(_ => { }, typeof(BusinessMappingProfile).Assembly, typeof(ApiMappingProfile).Assembly);
        return services.BuildServiceProvider().GetRequiredService<IMapper>();
    }

    [Fact]
    public void UpdateMemberRequest_maps_MembershipExemptManual_to_UpdateMemberCommand()
    {
        var mapper = CreateMapper();
        var req = new UpdateMemberRequest { Id = 1, Name = "A", MembershipExemptManual = true };
        var cmd = mapper.Map<UpdateMemberCommand>(req);
        cmd.MembershipExemptManual.Should().BeTrue();
    }

    [Fact]
    public void UpdateMemberCommand_maps_MembershipExemptManual_onto_MemberDb()
    {
        var mapper = CreateMapper();
        var existing = new MemberDb { Name = "A", Surname = "B", MembershipExemptManual = false };
        var cmd = new UpdateMemberCommand { Id = 1, MembershipExemptManual = true };
        mapper.Map(cmd, existing);
        existing.MembershipExemptManual.Should().BeTrue();
    }
}
