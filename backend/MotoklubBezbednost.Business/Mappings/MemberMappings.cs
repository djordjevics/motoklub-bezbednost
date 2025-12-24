using System.Collections.Generic;
using System.Linq;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Business.Mappings;

public static class MemberMappings
{
    public static MemberDto ToDto(this MemberDb entity)
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
            MemberType = entity.MemberType?.ToDto(),
            Motorcycles = entity.Motorcycles.Select(m => m.ToDto()).ToList(),
            Equipment = entity.Equipment?.ToDto(),
            Trainings = entity.Trainings.Select(t => t.ToDto()).ToList(),
            MembershipPayments = entity.MembershipPayments.Select(mp => mp.ToDto()).ToList(),
            Comments = entity.Comments.Select(c => c.ToDto()).ToList(),
            Tags = entity.Tags.Select(t => t.ToDto()).ToList()
        };
    }

    public static IEnumerable<MemberDto> ToDto(this IEnumerable<MemberDb> entities)
        => entities.Select(e => e.ToDto());

    public static MemberDb ToEntity(this MemberDto dto, MemberDb? existing = null)
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


