using MotoklubBezbednost.API.Models;
using MotoklubBezbednost.API.Repositories;

namespace MotoklubBezbednost.API.Services;

public class MotorcycleService : IMotorcycleService
{
    private readonly IMotorcycleRepository _motorcycleRepository;

    public MotorcycleService(IMotorcycleRepository motorcycleRepository)
    {
        _motorcycleRepository = motorcycleRepository;
    }

    public async Task<IEnumerable<Motorcycle>> GetAllMotorcyclesAsync()
    {
        return await _motorcycleRepository.GetAllWithMemberAsync();
    }

    public async Task<Motorcycle?> GetMotorcycleByIdAsync(int id)
    {
        return await _motorcycleRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Motorcycle>> GetMotorcyclesByMemberIdAsync(int memberId)
    {
        return await _motorcycleRepository.GetByMemberIdAsync(memberId);
    }

    public async Task<Motorcycle> CreateMotorcycleAsync(Motorcycle motorcycle)
    {
        return await _motorcycleRepository.AddAsync(motorcycle);
    }

    public async Task UpdateMotorcycleAsync(Motorcycle motorcycle)
    {
        await _motorcycleRepository.UpdateAsync(motorcycle);
    }

    public async Task DeleteMotorcycleAsync(int id)
    {
        await _motorcycleRepository.DeleteAsync(id);
    }
}

