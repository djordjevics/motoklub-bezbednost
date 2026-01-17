using System.Collections.Generic;
using System.Linq;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Business.Mappings;

/// <summary>
/// Concrete mapper for MemberDb &lt;-&gt; MemberDto following the generic mapper interfaces.
/// </summary>
public class MemberMapper : ITwoWayMapper<MemberDb, MemberDto>
{
    public MemberDto Map(MemberDb entity)
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
            CreationTimestamp = entity.CreationTimestamp,
            LastModificationTimestamp = entity.LastModificationTimestamp,
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
                RegisterPlate = m.RegisterPlate,
                CreationTimestamp = m.CreationTimestamp,
                LastModificationTimestamp = m.LastModificationTimestamp
            }).ToList() ?? new List<MotorcycleDto>(),
            Equipment = entity.Equipment != null ? new EquipmentDto
            {
                Id = entity.Equipment.Id,
                Pants = entity.Equipment.Pants,
                Jacket = entity.Equipment.Jacket,
                Vest = entity.Equipment.Vest,
                WorkShirt = entity.Equipment.WorkShirt,
                FormalShirt = entity.Equipment.FormalShirt,
                Note = entity.Equipment.Note,
                CreationTimestamp = entity.Equipment.CreationTimestamp,
                LastModificationTimestamp = entity.Equipment.LastModificationTimestamp
            } : null,
            Trainings = entity.Trainings?.Select(t => new TrainingDto
            {
                Id = t.Id,
                IsCertificateIssued = t.IsCertificateIssued,
                Note = t.Note,
                CreationTimestamp = t.CreationTimestamp,
                LastModificationTimestamp = t.LastModificationTimestamp
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

    public MemberDb Map(MemberDto dto)
    {
        return new MemberDb
        {
            Name = dto.Name,
            Surname = dto.Surname,
            Jmbg = dto.Jmbg,
            DateOfBirth = dto.DateOfBirth,
            Workplace = dto.Workplace,
            MobilePhone = dto.MobilePhone,
            EmergencyContact = dto.EmergencyContact,
            EmergencyContactPhone = dto.EmergencyContactPhone,
            Email = dto.Email,
            Address = dto.Address,
            RegisteredOn = dto.RegisteredOn,
            Note = dto.Note
        };
    }
}
