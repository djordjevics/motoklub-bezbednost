using MotoklubBezbednost.API.Models;

namespace MotoklubBezbednost.API.Services;

public interface IMotorcycleService
{
    Task<IEnumerable<Motorcycle>> GetAllMotorcyclesAsync();
    Task<Motorcycle?> GetMotorcycleByIdAsync(int id);
    Task<IEnumerable<Motorcycle>> GetMotorcyclesByMemberIdAsync(int memberId);
    Task<Motorcycle> CreateMotorcycleAsync(Motorcycle motorcycle);
    Task UpdateMotorcycleAsync(Motorcycle motorcycle);
    Task DeleteMotorcycleAsync(int id);
}

