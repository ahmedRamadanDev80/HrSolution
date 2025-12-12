using System.Collections.Generic;
using System.Threading.Tasks;
using Tamweely.Application.DTOs;
using Tamweely.Application.Features.Employees;
namespace Tamweely.Application.Interfaces;
public interface IDepartmentRepository
{
    
    Task<IReadOnlyList<DepartmentDto>> GetAllAsync(bool includeInactive = false);
    Task<int> CreateAsync(DepartmentDto dto);
    Task UpdateAsync(int id, DepartmentDto dto);
    Task SoftDeleteAsync(int id);
}