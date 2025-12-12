using AutoMapper;
using Tamweely.Domain.Entities;
using Tamweely.Application.DTOs;
namespace Tamweely.Application.Mapping;
public class MappingProfile : Profile 
{
    public MappingProfile() 
    {
        CreateMap<Employee, EmployeeDto>().ForMember(d => d.FullName, o => o.MapFrom(s => s.FullName)).ForMember(d => d.DepartmentName, o => o.MapFrom(s => s.Department != null ? s.Department.Name : string.Empty)).ForMember(d => d.JobTitleName, o => o.MapFrom(s => s.JobTitle != null ? s.JobTitle.Name : string.Empty));
        CreateMap<CreateOrEditEmployeeDto, Employee>();
        CreateMap<Department, DepartmentDto>().ReverseMap(); 
    } 
}