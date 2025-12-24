using MotoklubBezbednost.API.Requests;
using MotoklubBezbednost.Business.Cqrs.Members.Commands;
using MotoklubBezbednost.Business.Cqrs.Members.Queries;
using MotoklubBezbednost.Business.Mappings;

namespace MotoklubBezbednost.API.Mappers;

/// <summary>
/// Injectable mapper for Member API requests to CQRS commands/queries.
/// </summary>
public sealed class MemberRequestMapper :
    IOneWayMapper<GetAllMembersRequest, GetAllMembersQuery>,
    IOneWayMapper<CreateMemberRequest, CreateMemberCommand>,
    IOneWayMapper<UpdateMemberRequest, UpdateMemberCommand>,
    IOneWayMapper<DeleteMemberRequest, DeleteMemberCommand>,
    IOneWayMapper<GetMemberByIdRequest, GetMemberByIdQuery>,
    IOneWayMapper<SearchMembersRequest, SearchMembersQuery>
{
    // IOneWayMapper<GetAllMembersRequest, GetAllMembersQuery>
    GetAllMembersQuery IOneWayMapper<GetAllMembersRequest, GetAllMembersQuery>.Map(GetAllMembersRequest source)
        => new GetAllMembersQuery();

    // IOneWayMapper<CreateMemberRequest, CreateMemberCommand>
    CreateMemberCommand IOneWayMapper<CreateMemberRequest, CreateMemberCommand>.Map(CreateMemberRequest source)
        => new CreateMemberCommand
        {
            Name = source.Name,
            Surname = source.Surname,
            Jmbg = source.Jmbg,
            DateOfBirth = source.DateOfBirth,
            Workplace = source.Workplace,
            MobilePhone = source.MobilePhone,
            EmergencyContact = source.EmergencyContact,
            EmergencyContactPhone = source.EmergencyContactPhone,
            Email = source.Email,
            Address = source.Address,
            Note = source.Note,
            CreationTimestamp = DateTime.UtcNow
        };

    // IOneWayMapper<UpdateMemberRequest, UpdateMemberCommand>
    UpdateMemberCommand IOneWayMapper<UpdateMemberRequest, UpdateMemberCommand>.Map(UpdateMemberRequest source)
        => new UpdateMemberCommand
        {
            Id = source.Id,
            Name = source.Name,
            Surname = source.Surname,
            Jmbg = source.Jmbg,
            DateOfBirth = source.DateOfBirth,
            Workplace = source.Workplace,
            MobilePhone = source.MobilePhone,
            EmergencyContact = source.EmergencyContact,
            EmergencyContactPhone = source.EmergencyContactPhone,
            Email = source.Email,
            Address = source.Address,
            Note = source.Note,
            LastModificationTimestamp = DateTime.UtcNow
        };

    // IOneWayMapper<DeleteMemberRequest, DeleteMemberCommand>
    DeleteMemberCommand IOneWayMapper<DeleteMemberRequest, DeleteMemberCommand>.Map(DeleteMemberRequest source)
        => new DeleteMemberCommand { Id = source.Id };

    // IOneWayMapper<GetMemberByIdRequest, GetMemberByIdQuery>
    GetMemberByIdQuery IOneWayMapper<GetMemberByIdRequest, GetMemberByIdQuery>.Map(GetMemberByIdRequest source)
        => new GetMemberByIdQuery { Id = source.Id };

    // IOneWayMapper<SearchMembersRequest, SearchMembersQuery>
    SearchMembersQuery IOneWayMapper<SearchMembersRequest, SearchMembersQuery>.Map(SearchMembersRequest source)
        => new SearchMembersQuery { Query = source.Query };
}
