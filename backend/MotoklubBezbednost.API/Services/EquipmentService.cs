using MotoklubBezbednost.API.Models;
using MotoklubBezbednost.API.Repositories;

namespace MotoklubBezbednost.API.Services;

public class EquipmentService : IEquipmentService
{
    private readonly IEquipmentRepository _equipmentRepository;

    public EquipmentService(IEquipmentRepository equipmentRepository)
    {
        _equipmentRepository = equipmentRepository;
    }

    public async Task<IEnumerable<Equipment>> GetAllEquipmentAsync()
    {
        return await _equipmentRepository.GetAllWithMemberAsync();
    }

    public async Task<Equipment?> GetEquipmentByIdAsync(int id)
    {
        return await _equipmentRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Equipment>> GetEquipmentByMemberIdAsync(int memberId)
    {
        var equipment = await _equipmentRepository.GetByMemberIdAsync(memberId);
        return equipment != null ? new[] { equipment } : Enumerable.Empty<Equipment>();
    }

    public async Task<Equipment> CreateEquipmentAsync(Equipment equipment)
    {
        return await _equipmentRepository.AddAsync(equipment);
    }

    public async Task UpdateEquipmentAsync(Equipment equipment)
    {
        await _equipmentRepository.UpdateAsync(equipment);
    }

    public async Task DeleteEquipmentAsync(int id)
    {
        await _equipmentRepository.DeleteAsync(id);
    }
}

