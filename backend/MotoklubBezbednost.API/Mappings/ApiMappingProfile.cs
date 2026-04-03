using AutoMapper;
using MotoklubBezbednost.API.Models.Requests;
using MotoklubBezbednost.API.Models.Responses;
using MotoklubBezbednost.Business.Cqrs.Equipment.Commands;
using MotoklubBezbednost.Business.Cqrs.Equipment.Queries;
using MotoklubBezbednost.Business.Cqrs.Members.Commands;
using MotoklubBezbednost.Business.Cqrs.Members.Queries;
using MotoklubBezbednost.Business.Cqrs.Motorcycles.Commands;
using MotoklubBezbednost.Business.Cqrs.Motorcycles.Queries;
using MotoklubBezbednost.Business.Cqrs.Trainings.Commands;
using MotoklubBezbednost.Business.Cqrs.Trainings.Queries;
using MotoklubBezbednost.Business.Dtos;

namespace MotoklubBezbednost.API.Mappings;

public sealed class ApiMappingProfile : Profile
{
    public ApiMappingProfile()
    {
        CreateMap<CreateMemberRequest, CreateMemberCommand>();
        CreateMap<UpdateMemberRequest, UpdateMemberCommand>();
        CreateMap<GetMemberByIdRequest, GetMemberByIdQuery>();
        CreateMap<SearchMembersRequest, SearchMembersQuery>()
            .ForMember(d => d.Query, o => o.MapFrom(s => s.Query ?? string.Empty));

        CreateMap<CreateEquipmentRequest, CreateEquipmentCommand>();
        CreateMap<UpdateEquipmentRequest, UpdateEquipmentCommand>();
        CreateMap<GetEquipmentByIdRequest, GetEquipmentByIdQuery>();
        CreateMap<GetEquipmentByMemberRequest, GetEquipmentByMemberQuery>();

        CreateMap<CreateMotorcycleRequest, CreateMotorcycleCommand>();
        CreateMap<UpdateMotorcycleRequest, UpdateMotorcycleCommand>();
        CreateMap<GetMotorcycleByIdRequest, GetMotorcycleByIdQuery>();
        CreateMap<GetMotorcyclesByMemberRequest, GetMotorcyclesByMemberQuery>();

        CreateMap<CreateTrainingRequest, CreateTrainingCommand>();
        CreateMap<CreateTrainingSessionRequest, CreateTrainingSessionCommand>();
        CreateMap<UpdateTrainingSessionRequest, UpdateTrainingSessionCommand>();
        CreateMap<GetTrainingByIdRequest, GetTrainingByIdQuery>();
        CreateMap<GetTrainingSessionByIdRequest, GetTrainingSessionByIdQuery>();
        CreateMap<GetTrainingsByMemberRequest, GetTrainingsByMemberQuery>();

        CreateMap<PaymentTypeDto, PaymentTypeResponse>();
        CreateMap<MembershipPaymentDto, MembershipPaymentResponse>();
        CreateMap<CommentDto, CommentResponse>();
        CreateMap<TagDto, TagResponse>();
        CreateMap<MemberTypeDto, MemberTypeResponse>();
        CreateMap<LevelDto, LevelResponse>();
        CreateMap<MotorcycleDto, MotorcycleResponse>();
        CreateMap<EquipmentDto, EquipmentResponse>();
        CreateMap<TrainingSessionDto, TrainingSessionResponse>();
        CreateMap<TrainingDto, TrainingResponse>();
        CreateMap<MemberDto, MemberResponse>();
    }
}
