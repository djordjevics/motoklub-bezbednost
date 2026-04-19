using AutoMapper;
using MotoklubBezbednost.Business.Cqrs.Equipment.Commands;
using MotoklubBezbednost.Business.Cqrs.Members.Commands;
using MotoklubBezbednost.Business.Cqrs.Motorcycles.Commands;
using MotoklubBezbednost.Business.Cqrs.Trainings.Commands;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Business.Mappings;

public sealed class BusinessMappingProfile : Profile
{
    public BusinessMappingProfile()
    {
        // --- Entity -> Dto ---
        CreateMap<MemberTypeDb, MemberTypeDto>();
        CreateMap<PaymentTypeDb, PaymentTypeDto>();
        CreateMap<LevelDb, LevelDto>();
        CreateMap<MotorcycleDb, MotorcycleDto>();
        CreateMap<EquipmentDb, EquipmentDto>();
        CreateMap<CommentDb, CommentDto>();
        CreateMap<TagDb, TagDto>();

        CreateMap<MembershipPaymentDb, MembershipPaymentDto>();

        CreateMap<TrainingDb, TrainingDto>()
            .ForMember(d => d.Member, o => o.Ignore())
            .ForMember(d => d.Motorcycle, o => o.Ignore())
            .ForMember(d => d.TrainingSession, o => o.Ignore());

        CreateMap<TrainingSessionDb, TrainingSessionDto>();

        CreateMap<MemberDb, MemberDto>()
            .ForMember(d => d.Motorcycles, o => o.MapFrom(s => s.Motorcycles ?? Array.Empty<MotorcycleDb>()));

        // --- Create commands -> entity ---
        CreateMap<CreateMemberCommand, MemberDb>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.RegisteredOn, o => o.MapFrom(_ => DateTime.UtcNow))
            .ForMember(d => d.CreationTimestamp, o => o.MapFrom(_ => DateTime.UtcNow))
            .ForMember(d => d.LastModificationTimestamp, o => o.Ignore())
            .ForMember(d => d.MemberTypeId, o => o.Ignore())
            .ForMember(d => d.MemberType, o => o.Ignore())
            .ForMember(d => d.Motorcycles, o => o.Ignore())
            .ForMember(d => d.Equipment, o => o.Ignore())
            .ForMember(d => d.Trainings, o => o.Ignore())
            .ForMember(d => d.MembershipPayments, o => o.Ignore())
            .ForMember(d => d.Comments, o => o.Ignore())
            .ForMember(d => d.Tags, o => o.Ignore());

        CreateMap<CreateEquipmentCommand, EquipmentDb>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.CreationTimestamp, o => o.MapFrom(_ => DateTime.UtcNow))
            .ForMember(d => d.LastModificationTimestamp, o => o.Ignore())
            .ForMember(d => d.Member, o => o.Ignore());

        CreateMap<CreateMotorcycleCommand, MotorcycleDb>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.CreationTimestamp, o => o.MapFrom(_ => DateTime.UtcNow))
            .ForMember(d => d.LastModificationTimestamp, o => o.Ignore())
            .ForMember(d => d.Member, o => o.Ignore())
            .ForMember(d => d.Trainings, o => o.Ignore());

        CreateMap<CreateTrainingCommand, TrainingDb>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.CreationTimestamp, o => o.MapFrom(_ => DateTime.UtcNow))
            .ForMember(d => d.LastModificationTimestamp, o => o.Ignore())
            .ForMember(d => d.Member, o => o.Ignore())
            .ForMember(d => d.Motorcycle, o => o.Ignore())
            .ForMember(d => d.TrainingSession, o => o.Ignore());

        CreateMap<CreateTrainingSessionCommand, TrainingSessionDb>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.CreationTimestamp, o => o.MapFrom(_ => DateTime.UtcNow))
            .ForMember(d => d.LastModificationTimestamp, o => o.Ignore())
            .ForMember(d => d.Level, o => o.Ignore())
            .ForMember(d => d.Trainings, o => o.Ignore());

        // --- Update commands -> tracked entity ---
        CreateMap<UpdateMemberCommand, MemberDb>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.RegisteredOn, o => o.Ignore())
            .ForMember(d => d.CreationTimestamp, o => o.Ignore())
            .ForMember(d => d.MemberTypeId, o => o.Ignore())
            .ForMember(d => d.MemberType, o => o.Ignore())
            .ForMember(d => d.Motorcycles, o => o.Ignore())
            .ForMember(d => d.Equipment, o => o.Ignore())
            .ForMember(d => d.Trainings, o => o.Ignore())
            .ForMember(d => d.MembershipPayments, o => o.Ignore())
            .ForMember(d => d.Comments, o => o.Ignore())
            .ForMember(d => d.Tags, o => o.Ignore())
            .ForMember(d => d.Name, o => { o.Condition((src, _, __) => src.Name != null); o.MapFrom(s => s.Name); })
            .ForMember(d => d.Surname, o => { o.Condition((src, _, __) => src.Surname != null); o.MapFrom(s => s.Surname); })
            .ForMember(d => d.Jmbg, o => { o.Condition((src, _, __) => src.Jmbg != null); o.MapFrom(s => s.Jmbg); })
            .ForMember(d => d.DateOfBirth, o => { o.Condition((src, _, __) => src.DateOfBirth != null); o.MapFrom(s => s.DateOfBirth); })
            .ForMember(d => d.Workplace, o => { o.Condition((src, _, __) => src.Workplace != null); o.MapFrom(s => s.Workplace); })
            .ForMember(d => d.MobilePhone, o => { o.Condition((src, _, __) => src.MobilePhone != null); o.MapFrom(s => s.MobilePhone); })
            .ForMember(d => d.EmergencyContact, o => { o.Condition((src, _, __) => src.EmergencyContact != null); o.MapFrom(s => s.EmergencyContact); })
            .ForMember(d => d.EmergencyContactPhone, o => { o.Condition((src, _, __) => src.EmergencyContactPhone != null); o.MapFrom(s => s.EmergencyContactPhone); })
            .ForMember(d => d.Email, o => { o.Condition((src, _, __) => src.Email != null); o.MapFrom(s => s.Email); })
            .ForMember(d => d.Address, o => { o.Condition((src, _, __) => src.Address != null); o.MapFrom(s => s.Address); })
            .ForMember(d => d.Note, o => { o.Condition((src, _, __) => src.Note != null); o.MapFrom(s => s.Note); })
            .ForMember(d => d.LastModificationTimestamp, o => o.MapFrom(_ => DateTime.UtcNow));

        CreateMap<UpdateEquipmentCommand, EquipmentDb>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.MemberId, o => o.Ignore())
            .ForMember(d => d.Member, o => o.Ignore())
            .ForMember(d => d.CreationTimestamp, o => o.Ignore())
            .ForMember(d => d.Note, o => { o.Condition(src => src.Note != null); o.MapFrom(s => s.Note); })
            .ForMember(d => d.LastModificationTimestamp, o => o.MapFrom(_ => DateTime.UtcNow));

        CreateMap<UpdateMotorcycleCommand, MotorcycleDb>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.MemberId, o => o.Ignore())
            .ForMember(d => d.Member, o => o.Ignore())
            .ForMember(d => d.Trainings, o => o.Ignore())
            .ForMember(d => d.CreationTimestamp, o => o.Ignore())
            .ForMember(d => d.BrandName, o => { o.Condition((src, _, __) => src.BrandName != null); o.MapFrom(s => s.BrandName); })
            .ForMember(d => d.CommercialName, o => { o.Condition((src, _, __) => src.CommercialName != null); o.MapFrom(s => s.CommercialName); })
            .ForMember(d => d.ModelName, o => { o.Condition((src, _, __) => src.ModelName != null); o.MapFrom(s => s.ModelName); })
            .ForMember(d => d.EngineDisplacment, o => { o.Condition((src, _, __) => src.EngineDisplacment != null); o.MapFrom(s => s.EngineDisplacment); })
            .ForMember(d => d.EnginePower, o => { o.Condition((src, _, __) => src.EnginePower != null); o.MapFrom(s => s.EnginePower); })
            .ForMember(d => d.Color, o => { o.Condition((src, _, __) => src.Color != null); o.MapFrom(s => s.Color); })
            .ForMember(d => d.RegisterPlate, o => { o.Condition((src, _, __) => src.RegisterPlate != null); o.MapFrom(s => s.RegisterPlate); })
            .ForMember(d => d.LastModificationTimestamp, o => o.MapFrom(_ => DateTime.UtcNow));

        CreateMap<UpdateTrainingSessionCommand, TrainingSessionDb>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.Level, o => o.Ignore())
            .ForMember(d => d.Trainings, o => o.Ignore())
            .ForMember(d => d.CreationTimestamp, o => o.Ignore())
            .ForMember(d => d.TheoryDate, o => { o.Condition((src, _, __) => src.TheoryDate != null); o.MapFrom(s => s.TheoryDate); })
            .ForMember(d => d.PolygonDate, o => { o.Condition((src, _, __) => src.PolygonDate != null); o.MapFrom(s => s.PolygonDate); })
            .ForMember(d => d.City, o => { o.Condition((src, _, __) => src.City != null); o.MapFrom(s => s.City); })
            .ForMember(d => d.Price, o => { o.Condition((src, _, __) => src.Price != null); o.MapFrom(s => s.Price); })
            .ForMember(d => d.Instructors, o => { o.Condition((src, _, __) => src.Instructors != null); o.MapFrom(s => s.Instructors); })
            .ForMember(d => d.Note, o => { o.Condition((src, _, __) => src.Note != null); o.MapFrom(s => s.Note); })
            .ForMember(d => d.LevelId, o => { o.Condition((src, _, __) => src.LevelId != null); o.MapFrom(s => s.LevelId); })
            .ForMember(d => d.LastModificationTimestamp, o => o.MapFrom(_ => DateTime.UtcNow));
    }
}
