using System.Collections.Generic;
using System.Linq;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Business.Mappings;

/// <summary>
/// Concrete mapper for MemberDb &lt;-&gt; MemberDto following the generic mapper interfaces.
/// </summary>
public class MemberMapper : ITwoWayDbMapper<MemberDb, MemberDto>
{
    // IOneWayMapper<MemberDb, MemberDto>.Map
    public MemberDto Map(MemberDb source) => ToDto(source);

    // IOneWayMapper<MemberDto, MemberDb>.Map
    public MemberDb Map(MemberDto source) => ToEntity(source, null);

    // IDbToDtoMapper<MemberDb, MemberDto>
    public MemberDto ToDto(MemberDb entity)
    {
        return new MemberDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Surname = entity.Surname,
            Jmbg = entity.Jmbg,
            DateOfBirth = entity.DateOfBirth,
            Workplace = entity.Workplace,
            MobilePhone = entity.MobilePhone,
            EmergencyContact = entity.EmergencyContact,
            EmergencyContactPhone = entity.EmergencyContactPhone,
            Email = entity.Email,
            Address = entity.Address,
            RegisteredOn = entity.RegisteredOn,
            Note = entity.Note,
            MemberType = entity.MemberType != null ? new MemberTypeDto
            {
                Id = entity.MemberType.Id,
                Prefix = entity.MemberType.Prefix,
                TypeName = entity.MemberType.TypeName,
                Color = entity.MemberType.Color,
                PaidMembership = entity.MemberType.PaidMembership
            } : null,
            Motorcycles = entity.Motorcycles?.Select(m => new MotorcycleDto
            {
                Id = m.Id,
                BrandName = m.BrandName,
                CommercialName = m.CommercialName,
                ModelName = m.ModelName,
                EngineDisplacment = m.EngineDisplacment,
                EnginePower = m.EnginePower,
                Color = m.Color,
                RegisterPlate = m.RegisterPlate
            }).ToList() ?? new List<MotorcycleDto>(),
            Equipment = entity.Equipment != null ? new EquipmentDto
            {
                Id = entity.Equipment.Id,
                Pants = entity.Equipment.Pants,
                Jacket = entity.Equipment.Jacket,
                Vest = entity.Equipment.Vest,
                WorkShirt = entity.Equipment.WorkShirt,
                FormalShirt = entity.Equipment.FormalShirt,
                Note = entity.Equipment.Note
            } : null,
            Trainings = entity.Trainings?.Select(t => new TrainingDto
            {
                Id = t.Id,
                IsCertificateIssued = t.IsCertificateIssued,
                Note = t.Note
            }).ToList() ?? new List<TrainingDto>(),
            MembershipPayments = entity.MembershipPayments?.Select(mp => new MembershipPaymentDto
            {
                Id = mp.Id,
                Amount = mp.Amount,
                PaymentDate = mp.PaymentDate,
                PaymentForYear = mp.PaymentForYear,
                Note = mp.Note,
                PaymentType = mp.PaymentType != null ? new PaymentTypeDto
                {
                    Id = mp.PaymentType.Id,
                    Type = mp.PaymentType.Type,
                    Description = mp.PaymentType.Description
                } : null
            }).ToList() ?? new List<MembershipPaymentDto>(),
            Comments = entity.Comments?.Select(c => new CommentDto
            {
                Id = c.Id,
                CreationTime = c.CreationTime,
                EditTime = c.EditTime,
                CommentText = c.CommentText
            }).ToList() ?? new List<CommentDto>(),
            Tags = entity.Tags?.Select(t => new TagDto
            {
                Id = t.Id,
                TagNumber = t.TagNumber,
                AssignedDate = t.AssignedDate,
                ValidFrom = t.ValidFrom,
                ValidTo = t.ValidTo
            }).ToList() ?? new List<TagDto>()
        };
    }

    public IEnumerable<MemberDto> ToDto(IEnumerable<MemberDb> entities) => entities.Select(ToDto);

    // ITwoWayDbMapper<MemberDb, MemberDto>
    public MemberDb ToEntity(MemberDto dto, MemberDb? existing = null)
    {
        var entity = existing ?? new MemberDb();

        entity.Name = dto.Name;
        entity.Surname = dto.Surname;
        entity.Jmbg = dto.Jmbg;
        entity.DateOfBirth = dto.DateOfBirth;
        entity.Workplace = dto.Workplace;
        entity.MobilePhone = dto.MobilePhone;
        entity.EmergencyContact = dto.EmergencyContact;
        entity.EmergencyContactPhone = dto.EmergencyContactPhone;
        entity.Email = dto.Email;
        entity.Address = dto.Address;
        entity.RegisteredOn = dto.RegisteredOn;
        entity.Note = dto.Note;

        return entity;
    }
}


