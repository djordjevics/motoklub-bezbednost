using System.Linq;
using MotoklubBezbednost.Business.Dtos;
using MotoklubBezbednost.Business.Mappings;
using MotoklubBezbednost.Data.Repositories;

namespace MotoklubBezbednost.Business.Services;

public class MotorcycleService : IMotorcycleService
{
    private readonly IMotorcycleRepository _motorcycleRepository;

    public MotorcycleService(IMotorcycleRepository motorcycleRepository)
    {
        _motorcycleRepository = motorcycleRepository;
    }

    public async Task<IEnumerable<MotorcycleDto>> GetAllMotorcyclesAsync()
        => (await _motorcycleRepository.GetAllWithMemberAsync()).ToDto();

    public async Task<MotorcycleDto?> GetMotorcycleByIdAsync(int id)
    {
        var entity = await _motorcycleRepository.GetByIdAsync(id);
        return entity?.ToDto();
    }

    public async Task<IEnumerable<MotorcycleDto>> GetMotorcyclesByMemberIdAsync(int memberId)
        => (await _motorcycleRepository.GetByMemberIdAsync(memberId)).ToDto();

    public async Task<MotorcycleDto> CreateMotorcycleAsync(MotorcycleDto motorcycle)
    {
        var entity = new MotoklubBezbednost.Data.Models.MotorcycleDb();
        entity.UpdateEntity(motorcycle);
        var created = await _motorcycleRepository.AddAsync(entity);
        return created.ToDto();
    }

    public async Task UpdateMotorcycleAsync(MotorcycleDto motorcycle)
    {
        var existing = await _motorcycleRepository.GetByIdAsync(motorcycle.Id);
        if (existing == null)
        {
            throw new InvalidOperationException($"Motorcycle with id {motorcycle.Id} not found.");
        }

        existing.UpdateEntity(motorcycle);
        await _motorcycleRepository.UpdateAsync(existing);
    }

    public async Task DeleteMotorcycleAsync(int id)
    {
        await _motorcycleRepository.DeleteAsync(id);
    }
}


