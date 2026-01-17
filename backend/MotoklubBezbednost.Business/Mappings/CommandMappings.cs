using MotoklubBezbednost.Business.Cqrs.Members.Commands;
using MotoklubBezbednost.Business.Cqrs.Equipment.Commands;
using MotoklubBezbednost.Business.Cqrs.Motorcycles.Commands;
using MotoklubBezbednost.Business.Cqrs.Trainings.Commands;
using MotoklubBezbednost.Data.Models;

namespace MotoklubBezbednost.Business.Mappings;

/// <summary>
/// Simple static mapping methods for Commands/Queries to DB models.
/// </summary>
public static class CommandMappings
{
    public static MemberDb ToDbModel(this CreateMemberCommand command)
    {
        return new MemberDb
        {
            Name = command.Name,
            Surname = command.Surname,
            Jmbg = command.Jmbg,
            DateOfBirth = command.DateOfBirth,
            Workplace = command.Workplace,
            MobilePhone = command.MobilePhone,
            EmergencyContact = command.EmergencyContact,
            EmergencyContactPhone = command.EmergencyContactPhone,
            Email = command.Email,
            Address = command.Address,
            Note = command.Note,
            RegisteredOn = DateTime.UtcNow,
            CreationTimestamp = DateTime.UtcNow
        };
    }

    public static void ApplyTo(this UpdateMemberCommand command, MemberDb entity)
    {
        if (command.Name is not null) entity.Name = command.Name;
        if (command.Surname is not null) entity.Surname = command.Surname;
        if (command.Jmbg is not null) entity.Jmbg = command.Jmbg;
        if (command.DateOfBirth is not null) entity.DateOfBirth = command.DateOfBirth;
        if (command.Workplace is not null) entity.Workplace = command.Workplace;
        if (command.MobilePhone is not null) entity.MobilePhone = command.MobilePhone;
        if (command.EmergencyContact is not null) entity.EmergencyContact = command.EmergencyContact;
        if (command.EmergencyContactPhone is not null) entity.EmergencyContactPhone = command.EmergencyContactPhone;
        if (command.Email is not null) entity.Email = command.Email;
        if (command.Address is not null) entity.Address = command.Address;
        if (command.Note is not null) entity.Note = command.Note;
        entity.LastModificationTimestamp = DateTime.UtcNow;
    }

    public static EquipmentDb ToDbModel(this CreateEquipmentCommand command)
    {
        return new EquipmentDb
        {
            Pants = command.Pants,
            Jacket = command.Jacket,
            Vest = command.Vest,
            WorkShirt = command.WorkShirt,
            FormalShirt = command.FormalShirt,
            Note = command.Note,
            MemberId = command.MemberId,
            CreationTimestamp = DateTime.UtcNow
        };
    }

    public static void ApplyTo(this UpdateEquipmentCommand command, EquipmentDb entity)
    {
        entity.Pants = command.Pants;
        entity.Jacket = command.Jacket;
        entity.Vest = command.Vest;
        entity.WorkShirt = command.WorkShirt;
        entity.FormalShirt = command.FormalShirt;
        if (command.Note is not null) entity.Note = command.Note;
        entity.LastModificationTimestamp = DateTime.UtcNow;
    }

    public static MotorcycleDb ToDbModel(this CreateMotorcycleCommand command)
    {
        return new MotorcycleDb
        {
            BrandName = command.BrandName,
            CommercialName = command.CommercialName,
            ModelName = command.ModelName,
            EngineDisplacment = command.EngineDisplacment,
            EnginePower = command.EnginePower,
            Color = command.Color,
            RegisterPlate = command.RegisterPlate,
            MemberId = command.MemberId,
            CreationTimestamp = DateTime.UtcNow
        };
    }

    public static void ApplyTo(this UpdateMotorcycleCommand command, MotorcycleDb entity)
    {
        if (command.BrandName is not null) entity.BrandName = command.BrandName;
        if (command.CommercialName is not null) entity.CommercialName = command.CommercialName;
        if (command.ModelName is not null) entity.ModelName = command.ModelName;
        if (command.EngineDisplacment is not null) entity.EngineDisplacment = command.EngineDisplacment;
        if (command.EnginePower is not null) entity.EnginePower = command.EnginePower;
        if (command.Color is not null) entity.Color = command.Color;
        if (command.RegisterPlate is not null) entity.RegisterPlate = command.RegisterPlate;
        entity.LastModificationTimestamp = DateTime.UtcNow;
    }

    public static TrainingDb ToDbModel(this CreateTrainingCommand command)
    {
        return new TrainingDb
        {
            IsCertificateIssued = command.IsCertificateIssued,
            Note = command.Note,
            MemberId = command.MemberId,
            MotorcycleId = command.MotorcycleId,
            TrainingSessionId = command.TrainingSessionId,
            CreationTimestamp = DateTime.UtcNow
        };
    }

    public static TrainingSessionDb ToDbModel(this CreateTrainingSessionCommand command)
    {
        return new TrainingSessionDb
        {
            TheoryDate = command.TheoryDate,
            PolygonDate = command.PolygonDate,
            City = command.City,
            Price = command.Price,
            Instructors = command.Instructors,
            Note = command.Note,
            LevelId = command.LevelId,
            CreationTimestamp = DateTime.UtcNow
        };
    }

    public static void ApplyTo(this UpdateTrainingSessionCommand command, TrainingSessionDb entity)
    {
        if (command.TheoryDate is not null) entity.TheoryDate = command.TheoryDate;
        if (command.PolygonDate is not null) entity.PolygonDate = command.PolygonDate;
        if (command.City is not null) entity.City = command.City;
        if (command.Price is not null) entity.Price = command.Price;
        if (command.Instructors is not null) entity.Instructors = command.Instructors;
        if (command.Note is not null) entity.Note = command.Note;
        if (command.LevelId is not null) entity.LevelId = command.LevelId;
        entity.LastModificationTimestamp = DateTime.UtcNow;
    }
}
