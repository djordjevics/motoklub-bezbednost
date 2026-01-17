using MotoklubBezbednost.API.Requests;
using MotoklubBezbednost.Business.Cqrs.Members.Commands;
using MotoklubBezbednost.Business.Cqrs.Members.Queries;
using MotoklubBezbednost.Business.Cqrs.Equipment.Commands;
using MotoklubBezbednost.Business.Cqrs.Equipment.Queries;
using MotoklubBezbednost.Business.Cqrs.Motorcycles.Commands;
using MotoklubBezbednost.Business.Cqrs.Motorcycles.Queries;
using MotoklubBezbednost.Business.Cqrs.Trainings.Commands;
using MotoklubBezbednost.Business.Cqrs.Trainings.Queries;

namespace MotoklubBezbednost.API.Mappers;

/// <summary>
/// Simple static mapping methods for API Requests to Commands/Queries.
/// </summary>
public static class RequestMappings
{
    // Members
    public static CreateMemberCommand ToCommand(this CreateMemberRequest request)
    {
        return new CreateMemberCommand
        {
            Name = request.Name,
            Surname = request.Surname,
            Jmbg = request.Jmbg,
            DateOfBirth = request.DateOfBirth,
            Workplace = request.Workplace,
            MobilePhone = request.MobilePhone,
            EmergencyContact = request.EmergencyContact,
            EmergencyContactPhone = request.EmergencyContactPhone,
            Email = request.Email,
            Address = request.Address,
            Note = request.Note
        };
    }

    public static UpdateMemberCommand ToCommand(this UpdateMemberRequest request)
    {
        return new UpdateMemberCommand
        {
            Id = request.Id,
            Name = request.Name,
            Surname = request.Surname,
            Jmbg = request.Jmbg,
            DateOfBirth = request.DateOfBirth,
            Workplace = request.Workplace,
            MobilePhone = request.MobilePhone,
            EmergencyContact = request.EmergencyContact,
            EmergencyContactPhone = request.EmergencyContactPhone,
            Email = request.Email,
            Address = request.Address,
            Note = request.Note
        };
    }

    public static GetMemberByIdQuery ToQuery(this GetMemberByIdRequest request)
    {
        return new GetMemberByIdQuery { Id = request.Id };
    }

    public static SearchMembersQuery ToQuery(this SearchMembersRequest request)
    {
        return new SearchMembersQuery { Query = request.Query };
    }

    // Equipment
    public static CreateEquipmentCommand ToCommand(this CreateEquipmentRequest request)
    {
        return new CreateEquipmentCommand
        {
            Pants = request.Pants,
            Jacket = request.Jacket,
            Vest = request.Vest,
            WorkShirt = request.WorkShirt,
            FormalShirt = request.FormalShirt,
            Note = request.Note,
            MemberId = request.MemberId
        };
    }

    public static UpdateEquipmentCommand ToCommand(this UpdateEquipmentRequest request)
    {
        return new UpdateEquipmentCommand
        {
            Id = request.Id,
            Pants = request.Pants,
            Jacket = request.Jacket,
            Vest = request.Vest,
            WorkShirt = request.WorkShirt,
            FormalShirt = request.FormalShirt,
            Note = request.Note
        };
    }

    public static GetEquipmentByIdQuery ToQuery(this GetEquipmentByIdRequest request)
    {
        return new GetEquipmentByIdQuery { Id = request.Id };
    }

    public static GetEquipmentByMemberQuery ToQuery(this GetEquipmentByMemberRequest request)
    {
        return new GetEquipmentByMemberQuery { MemberId = request.MemberId };
    }

    // Motorcycles
    public static CreateMotorcycleCommand ToCommand(this CreateMotorcycleRequest request)
    {
        return new CreateMotorcycleCommand
        {
            BrandName = request.BrandName,
            CommercialName = request.CommercialName,
            ModelName = request.ModelName,
            EngineDisplacment = request.EngineDisplacment,
            EnginePower = request.EnginePower,
            Color = request.Color,
            RegisterPlate = request.RegisterPlate,
            MemberId = request.MemberId
        };
    }

    public static UpdateMotorcycleCommand ToCommand(this UpdateMotorcycleRequest request)
    {
        return new UpdateMotorcycleCommand
        {
            Id = request.Id,
            BrandName = request.BrandName,
            CommercialName = request.CommercialName,
            ModelName = request.ModelName,
            EngineDisplacment = request.EngineDisplacment,
            EnginePower = request.EnginePower,
            Color = request.Color,
            RegisterPlate = request.RegisterPlate,
            MemberId = request.MemberId
        };
    }

    public static GetMotorcycleByIdQuery ToQuery(this GetMotorcycleByIdRequest request)
    {
        return new GetMotorcycleByIdQuery { Id = request.Id };
    }

    public static GetMotorcyclesByMemberQuery ToQuery(this GetMotorcyclesByMemberRequest request)
    {
        return new GetMotorcyclesByMemberQuery { MemberId = request.MemberId };
    }

    // Trainings
    public static CreateTrainingCommand ToCommand(this CreateTrainingRequest request)
    {
        return new CreateTrainingCommand
        {
            IsCertificateIssued = request.IsCertificateIssued,
            Note = request.Note,
            MemberId = request.MemberId,
            MotorcycleId = request.MotorcycleId,
            TrainingSessionId = request.TrainingSessionId
        };
    }

    public static CreateTrainingSessionCommand ToCommand(this CreateTrainingSessionRequest request)
    {
        return new CreateTrainingSessionCommand
        {
            TheoryDate = request.TheoryDate,
            PolygonDate = request.PolygonDate,
            City = request.City,
            Price = request.Price,
            Instructors = request.Instructors,
            Note = request.Note,
            LevelId = request.LevelId
        };
    }

    public static UpdateTrainingSessionCommand ToCommand(this UpdateTrainingSessionRequest request)
    {
        return new UpdateTrainingSessionCommand
        {
            Id = request.Id,
            TheoryDate = request.TheoryDate,
            PolygonDate = request.PolygonDate,
            City = request.City,
            Price = request.Price,
            Instructors = request.Instructors,
            Note = request.Note,
            LevelId = request.LevelId
        };
    }

    public static GetTrainingByIdQuery ToQuery(this GetTrainingByIdRequest request)
    {
        return new GetTrainingByIdQuery { Id = request.Id };
    }

    public static GetTrainingSessionByIdQuery ToQuery(this GetTrainingSessionByIdRequest request)
    {
        return new GetTrainingSessionByIdQuery { Id = request.Id };
    }

    public static GetTrainingsByMemberQuery ToQuery(this GetTrainingsByMemberRequest request)
    {
        return new GetTrainingsByMemberQuery { MemberId = request.MemberId };
    }
}
