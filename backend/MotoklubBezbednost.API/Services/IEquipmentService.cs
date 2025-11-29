using MotoklubBezbednost.API.Models;

namespace MotoklubBezbednost.API.Services;

public interface IEquipmentService
{
    Task<IEnumerable<Equipment>> GetAllEquipmentAsync();
    Task<Equipment?> GetEquipmentByIdAsync(int id);
    Task<IEnumerable<Equipment>> GetEquipmentByMemberIdAsync(int memberId);
    Task<Equipment> CreateEquipmentAsync(Equipment equipment);
    Task UpdateEquipmentAsync(Equipment equipment);
    Task DeleteEquipmentAsync(int id);
}

