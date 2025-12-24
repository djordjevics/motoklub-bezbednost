using MotoklubBezbednost.API.Requests;
using MotoklubBezbednost.Business.Cqrs.Trainings.Commands;
using MotoklubBezbednost.Business.Cqrs.Trainings.Queries;
using MotoklubBezbednost.Business.Mappings;

namespace MotoklubBezbednost.API.Mappers;

/// <summary>
/// Injectable mapper for Training API requests to CQRS commands/queries.
/// </summary>
public sealed class TrainingRequestMapper :
    IOneWayMapper<GetAllTrainingSessionsRequest, GetAllTrainingSessionsQuery>,
    IOneWayMapper<GetTrainingSessionByIdRequest, GetTrainingSessionByIdQuery>,
    IOneWayMapper<GetTrainingsByMemberRequest, GetTrainingsByMemberQuery>,
    IOneWayMapper<GetTrainingByIdRequest, GetTrainingByIdQuery>,
    IOneWayMapper<CreateTrainingSessionRequest, CreateTrainingSessionCommand>,
    IOneWayMapper<UpdateTrainingSessionRequest, UpdateTrainingSessionCommand>,
    IOneWayMapper<DeleteTrainingSessionRequest, DeleteTrainingSessionCommand>,
    IOneWayMapper<CreateTrainingRequest, CreateTrainingCommand>
{
    // IOneWayMapper<GetAllTrainingSessionsRequest, GetAllTrainingSessionsQuery>
    GetAllTrainingSessionsQuery IOneWayMapper<GetAllTrainingSessionsRequest, GetAllTrainingSessionsQuery>.Map(GetAllTrainingSessionsRequest source)
        => new GetAllTrainingSessionsQuery();

    // IOneWayMapper<GetTrainingSessionByIdRequest, GetTrainingSessionByIdQuery>
    GetTrainingSessionByIdQuery IOneWayMapper<GetTrainingSessionByIdRequest, GetTrainingSessionByIdQuery>.Map(GetTrainingSessionByIdRequest source)
        => new GetTrainingSessionByIdQuery { Id = source.Id };

    // IOneWayMapper<GetTrainingsByMemberRequest, GetTrainingsByMemberQuery>
    GetTrainingsByMemberQuery IOneWayMapper<GetTrainingsByMemberRequest, GetTrainingsByMemberQuery>.Map(GetTrainingsByMemberRequest source)
        => new GetTrainingsByMemberQuery { MemberId = source.MemberId };

    // IOneWayMapper<GetTrainingByIdRequest, GetTrainingByIdQuery>
    GetTrainingByIdQuery IOneWayMapper<GetTrainingByIdRequest, GetTrainingByIdQuery>.Map(GetTrainingByIdRequest source)
        => new GetTrainingByIdQuery { Id = source.Id };

    // IOneWayMapper<CreateTrainingSessionRequest, CreateTrainingSessionCommand>
    CreateTrainingSessionCommand IOneWayMapper<CreateTrainingSessionRequest, CreateTrainingSessionCommand>.Map(CreateTrainingSessionRequest source)
        => new CreateTrainingSessionCommand
        {
            TheoryDate = source.TheoryDate,
            PolygonDate = source.PolygonDate,
            City = source.City,
            Price = source.Price,
            Instructors = source.Instructors,
            Note = source.Note,
            LevelId = source.LevelId,
            CreationTimestamp = DateTime.UtcNow
        };

    // IOneWayMapper<UpdateTrainingSessionRequest, UpdateTrainingSessionCommand>
    UpdateTrainingSessionCommand IOneWayMapper<UpdateTrainingSessionRequest, UpdateTrainingSessionCommand>.Map(UpdateTrainingSessionRequest source)
        => new UpdateTrainingSessionCommand
        {
            Id = source.Id,
            TheoryDate = source.TheoryDate,
            PolygonDate = source.PolygonDate,
            City = source.City,
            Price = source.Price,
            Instructors = source.Instructors,
            Note = source.Note,
            LevelId = source.LevelId,
            LastModificationTimestamp = DateTime.UtcNow
        };

    // IOneWayMapper<DeleteTrainingSessionRequest, DeleteTrainingSessionCommand>
    DeleteTrainingSessionCommand IOneWayMapper<DeleteTrainingSessionRequest, DeleteTrainingSessionCommand>.Map(DeleteTrainingSessionRequest source)
        => new DeleteTrainingSessionCommand { Id = source.Id };

    // IOneWayMapper<CreateTrainingRequest, CreateTrainingCommand>
    CreateTrainingCommand IOneWayMapper<CreateTrainingRequest, CreateTrainingCommand>.Map(CreateTrainingRequest source)
        => new CreateTrainingCommand
        {
            IsCertificateIssued = source.IsCertificateIssued,
            Note = source.Note,
            MemberId = source.MemberId,
            MotorcycleId = source.MotorcycleId,
            TrainingSessionId = source.TrainingSessionId,
            CreationTimestamp = DateTime.UtcNow
        };
}
