using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tamweely.Application.Interfaces;
using Tamweely.Application.DTOs;
using Tamweely.Infrastructure.Persistence;
using Tamweely.Infrastructure.Repositories;
using Tamweely.Infrastructure.Services;
using AutoMapper;
using Tamweely.Application.Mapping;
using FluentValidation;
using Tamweely.Application.Validators;

namespace Tamweely.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(opts => opts.UseSqlServer(config.GetConnectionString("DefaultConnection")));
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IJobRepository, JobRepository>();
        services.AddAutoMapper(cfg => {
            cfg.AddProfile<MappingProfile>();
        });
        services.AddScoped<IValidator<CreateOrEditEmployeeDto>, CreateOrEditEmployeeDtoValidator>();
        return services;
    }
}