using System.Collections.Generic;
using System.Threading.Tasks;
using Tamweely.Application.DTOs;
using Tamweely.Application.Features.Employees;
namespace Tamweely.Application.Interfaces;
public interface IEmployeeRepository 
{
    Task<(IReadOnlyList<EmployeeDto> Items, int Total)> SearchAsync(EmployeeQueryParams q);
    Task<EmployeeDto?> GetByIdAsync(int id); Task<int> CreateAsync(CreateOrEditEmployeeDto dto);
    Task UpdateAsync(int id, CreateOrEditEmployeeDto dto);
    Task SoftDeleteAsync(int id);
    Task<byte[]> ExportToExcelAsync(EmployeeQueryParams q);
}