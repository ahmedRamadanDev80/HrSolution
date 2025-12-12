using System;
namespace Tamweely.Application.Features.Employees;
public class EmployeeQueryParams 
{
    public string? Search { get; set; }
    public int? DepartmentId { get; set; }
    public int? JobTitleId { get; set; }
    public DateTime? DobFrom { get; set; }
    public DateTime? DobTo { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; }
    public bool SortDesc { get; set; } = false;
}