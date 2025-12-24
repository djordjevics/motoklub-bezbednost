using MotoklubBezbednost.Business.Dtos;

namespace MotoklubBezbednost.Business.Services;

public interface IEquipmentService
{
    Task<IEnumerable<EquipmentDto>> GetAllEquipmentAsync();
    Task<EquipmentDto?> GetEquipmentByIdAsync(int id);
    Task<IEnumerable<EquipmentDto>> GetEquipmentByMemberIdAsync(int memberId);
    Task<EquipmentDto> CreateEquipmentAsync(EquipmentDto equipment);
    Task UpdateEquipmentAsync(EquipmentDto equipment);
    Task DeleteEquipmentAsync(int id);
}


