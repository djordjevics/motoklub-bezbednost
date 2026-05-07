using AutoMapper;
using MotoklubBezbednost.Business.Cqrs.Comments.Commands;
using MotoklubBezbednost.Business.Cqrs.Equipment.Commands;
using MotoklubBezbednost.Business.Cqrs.Members.Commands;
using MotoklubBezbednost.Business.Cqrs.MembershipPayments.Commands;
using MotoklubBezbednost.Business.Cqrs.Motorcycles.Commands;
using MotoklubBezbednost.Business.Cqrs.Tags.Commands;
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
            .ForMember(d => d.Motorcycle, o => o.MapFrom(s => s.Motorcycle))
            .ForMember(d => d.TrainingSession, o => o.MapFrom(s => s.TrainingSession));

        CreateMap<TrainingSessionDb, TrainingSessionDto>();

        CreateMap<MemberDb, MemberDto>()
            .ForMember(d => d.Motorcycles, o => o.MapFrom(s => s.Motorcycles ?? Array.Empty<MotorcycleDb>()));

        // --- Create commands -> entity ---
        CreateMap<CreateMemberCommand, MemberDb>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.RegisteredOn, o => o.MapFrom(_ => DateTime.UtcNow))
            .ForMember(d => d.CreationTimestamp, o => o.MapFrom(_ => DateTime.UtcNow))
            .ForMember(d => d.LastModificationTimestamp, o => o.Ignore())
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
            .ForMember(d => d.LastModificationTimestamp, o => o.Ignore());

        CreateMap<CreateMotorcycleCommand, MotorcycleDb>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.CreationTimestamp, o => o.MapFrom(_ => DateTime.UtcNow))
            .ForMember(d => d.LastModificationTimestamp, o => o.Ignore())
            .ForMember(d => d.Trainings, o => o.Ignore());

        CreateMap<CreateTrainingCommand, TrainingDb>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.CreationTimestamp, o => o.MapFrom(_ => DateTime.UtcNow))
            .ForMember(d => d.LastModificationTimestamp, o => o.Ignore())
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
            .ForMember(d => d.MemberTypeId, o => { o.Condition((src, _, __) => src.MemberTypeId != null); o.MapFrom(s => s.MemberTypeId); })
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
            .ForMember(d => d.MembershipExemptManual, o =>
            {
                o.Condition((src, _, __) => src.MembershipExemptManual != null);
                o.MapFrom(s => s.MembershipExemptManual!.Value);
            })
            .ForMember(d => d.LastModificationTimestamp, o => o.MapFrom(_ => DateTime.UtcNow));

        CreateMap<UpdateEquipmentCommand, EquipmentDb>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.MemberId, o => o.Ignore())
            .ForMember(d => d.CreationTimestamp, o => o.Ignore())
            .ForMember(d => d.Note, o => { o.Condition(src => src.Note != null); o.MapFrom(s => s.Note); })
            .ForMember(d => d.LastModificationTimestamp, o => o.MapFrom(_ => DateTime.UtcNow));

        CreateMap<UpdateMotorcycleCommand, MotorcycleDb>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.MemberId, o => o.Ignore())
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

        CreateMap<UpdateTrainingCommand, TrainingDb>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.MemberId, o => o.Ignore())
            .ForMember(d => d.TrainingSessionId, o => o.Ignore())
            .ForMember(d => d.Motorcycle, o => o.Ignore())
            .ForMember(d => d.TrainingSession, o => o.Ignore())
            .ForMember(d => d.CreationTimestamp, o => o.Ignore())
            .ForMember(d => d.RepeatingAttendance, o => { o.Condition((src, _, __) => src.RepeatingAttendance != null); o.MapFrom(s => s.RepeatingAttendance!.Value); })
            .ForMember(d => d.IsCertificateIssued, o => { o.Condition((src, _, __) => src.IsCertificateIssued != null); o.MapFrom(s => s.IsCertificateIssued!.Value); })
            .ForMember(d => d.Note, o => { o.Condition((src, _, __) => src.Note != null); o.MapFrom(s => s.Note); })
            .ForMember(d => d.MotorcycleId, o => { o.Condition((src, _, __) => src.MotorcycleId != null); o.MapFrom(s => s.MotorcycleId!.Value); })
            .ForMember(d => d.LastModificationTimestamp, o => o.MapFrom(_ => DateTime.UtcNow));

        CreateMap<CreateTagCommand, TagDb>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.CreationTimestamp, o => o.MapFrom(_ => DateTime.UtcNow))
            .ForMember(d => d.LastModificationTimestamp, o => o.Ignore());

        CreateMap<UpdateTagCommand, TagDb>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.MemberId, o => o.Ignore())
            .ForMember(d => d.CreationTimestamp, o => o.Ignore())
            .ForMember(d => d.TagNumber, o => { o.Condition((src, _, __) => src.TagNumber != null); o.MapFrom(s => s.TagNumber); })
            .ForMember(d => d.AssignedDate, o => { o.Condition((src, _, __) => src.AssignedDate != null); o.MapFrom(s => s.AssignedDate); })
            .ForMember(d => d.ValidFrom, o => { o.Condition((src, _, __) => src.ValidFrom != null); o.MapFrom(s => s.ValidFrom); })
            .ForMember(d => d.ValidTo, o => { o.Condition((src, _, __) => src.ValidTo != null); o.MapFrom(s => s.ValidTo); })
            .ForMember(d => d.LastModificationTimestamp, o => o.MapFrom(_ => DateTime.UtcNow));

        CreateMap<CreateCommentCommand, CommentDb>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.CreationTime, o => o.Ignore())
            .ForMember(d => d.EditTime, o => o.Ignore())
            .ForMember(d => d.CreationTimestamp, o => o.MapFrom(_ => DateTime.UtcNow))
            .ForMember(d => d.LastModificationTimestamp, o => o.Ignore());

        CreateMap<UpdateCommentCommand, CommentDb>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.MemberId, o => o.Ignore())
            .ForMember(d => d.CreationTime, o => o.Ignore())
            .ForMember(d => d.EditTime, o => o.Ignore())
            .ForMember(d => d.CreationTimestamp, o => o.Ignore())
            .ForMember(d => d.CommentText, o => { o.Condition((src, _, __) => src.CommentText != null); o.MapFrom(s => s.CommentText); })
            .ForMember(d => d.LastModificationTimestamp, o => o.MapFrom(_ => DateTime.UtcNow));

        CreateMap<CreateMembershipPaymentCommand, MembershipPaymentDb>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.PaymentType, o => o.Ignore())
            .ForMember(d => d.CreationTimestamp, o => o.MapFrom(_ => DateTime.UtcNow))
            .ForMember(d => d.LastModificationTimestamp, o => o.Ignore());

        CreateMap<UpdateMembershipPaymentCommand, MembershipPaymentDb>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.MemberId, o => o.Ignore())
            .ForMember(d => d.PaymentType, o => o.Ignore())
            .ForMember(d => d.CreationTimestamp, o => o.Ignore())
            .ForMember(d => d.Amount, o => { o.Condition((src, _, __) => src.Amount != null); o.MapFrom(s => s.Amount); })
            .ForMember(d => d.PaymentDate, o => { o.Condition((src, _, __) => src.PaymentDate != null); o.MapFrom(s => s.PaymentDate); })
            .ForMember(d => d.PaymentForYear, o => { o.Condition((src, _, __) => src.PaymentForYear != null); o.MapFrom(s => s.PaymentForYear); })
            .ForMember(d => d.PaymentTypeId, o => { o.Condition((src, _, __) => src.PaymentTypeId != null); o.MapFrom(s => s.PaymentTypeId); })
            .ForMember(d => d.Note, o => { o.Condition((src, _, __) => src.Note != null); o.MapFrom(s => s.Note); })
            .ForMember(d => d.LastModificationTimestamp, o => o.MapFrom(_ => DateTime.UtcNow));
    }
}
