using MotoklubBezbednost.Business.Dtos;

namespace MotoklubBezbednost.Business.Services;

public interface IMotorcycleService
{
    Task<IEnumerable<MotorcycleDto>> GetAllMotorcyclesAsync();
    Task<MotorcycleDto?> GetMotorcycleByIdAsync(int id);
    Task<IEnumerable<MotorcycleDto>> GetMotorcyclesByMemberIdAsync(int memberId);
    Task<MotorcycleDto> CreateMotorcycleAsync(MotorcycleDto motorcycle);
    Task UpdateMotorcycleAsync(MotorcycleDto motorcycle);
    Task DeleteMotorcycleAsync(int id);
}


