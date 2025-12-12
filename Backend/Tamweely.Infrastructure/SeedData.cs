using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tamweely.Domain.Entities;
using Tamweely.Infrastructure.Persistence;

namespace Tamweely.Infrastructure;

public static class SeedData
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var db = serviceProvider.GetRequiredService<AppDbContext>();

        // --- Seed Roles ---
        var roles = new[] { "Admin", "User" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // --- Seed Admin User ---
        var adminUser = await userManager.FindByNameAsync("admin");
        if (adminUser == null)
        {
            adminUser = new AppUser
            {
                UserName = "admin",
                Email = "admin@tamweely.com",
                IsActive = true,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(adminUser, "Admin@123"); // default password
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }

        // --- Seed Departments ---
        if (!db.Departments.Any())
        {
            db.Departments.AddRange(
                new Department { Name = "HR", IsActive = true },
                new Department { Name = "IT", IsActive = true },
                new Department { Name = "Finance", IsActive = true }
            );
            await db.SaveChangesAsync();
        }

        // --- Seed JobTitles ---
        if (!db.JobTitles.Any())
        {
            db.JobTitles.AddRange(
                new JobTitle { Name = "Manager", IsActive = true },
                new JobTitle { Name = "Developer", IsActive = true },
                new JobTitle { Name = "Accountant", IsActive = true }
            );
            await db.SaveChangesAsync();
        }

        // --- Seed sample Employee ---
        if (!db.Employees.Any())
        {
            var hrDept = db.Departments.First(d => d.Name == "HR");
            var managerJob = db.JobTitles.First(j => j.Name == "Manager");

            db.Employees.Add(new Employee
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@tamweely.com",
                Mobile = "01234567890",
                DepartmentId = hrDept.Id,
                JobTitleId = managerJob.Id,
                DateOfBirth = new DateTime(1990, 1, 1),
                IsActive = true
            });

            await db.SaveChangesAsync();
        }
    }
}
