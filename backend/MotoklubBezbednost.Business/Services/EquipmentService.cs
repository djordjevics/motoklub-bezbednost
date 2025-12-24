using System.Linq;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Business.Mappings;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Services;

public class EquipmentService : IEquipmentService
{
    private readonly IEquipmentRepository _equipmentRepository;

    public EquipmentService(IEquipmentRepository equipmentRepository)
    {
        _equipmentRepository = equipmentRepository;
    }

    public async Task<IEnumerable<EquipmentDto>> GetAllEquipmentAsync()
        => (await _equipmentRepository.GetAllWithMemberAsync()).ToDto();

    public async Task<EquipmentDto?> GetEquipmentByIdAsync(int id)
    {
        var entity = await _equipmentRepository.GetByIdAsync(id);
        return entity?.ToDto();
    }

    public async Task<IEnumerable<EquipmentDto>> GetEquipmentByMemberIdAsync(int memberId)
    {
        var equipment = await _equipmentRepository.GetByMemberIdAsync(memberId);
        return equipment != null ? new[] { equipment.ToDto() } : Enumerable.Empty<EquipmentDto>();
    }

    public async Task<EquipmentDto> CreateEquipmentAsync(EquipmentDto equipment)
    {
        var entity = new MotoklubBezbednost.Data.Models.EquipmentDb();
        entity.UpdateEntity(equipment);
        var created = await _equipmentRepository.AddAsync(entity);
        return created.ToDto();
    }

    public async Task UpdateEquipmentAsync(EquipmentDto equipment)
    {
        var existing = await _equipmentRepository.GetByIdAsync(equipment.Id);
        if (existing == null)
        {
            throw new InvalidOperationException($"Equipment with id {equipment.Id} not found.");
        }

        existing.UpdateEntity(equipment);
        await _equipmentRepository.UpdateAsync(existing);
    }

    public async Task DeleteEquipmentAsync(int id)
    {
        await _equipmentRepository.DeleteAsync(id);
    }
}


