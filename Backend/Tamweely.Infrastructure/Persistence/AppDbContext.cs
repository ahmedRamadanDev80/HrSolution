using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tamweely.Domain.Entities;

namespace Tamweely.Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<AppUser>
{

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<JobTitle> JobTitles => Set<JobTitle>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) {

        base.OnModelCreating(modelBuilder); modelBuilder.Entity<Department>().HasIndex(d => d.Name).IsUnique();
        modelBuilder.Entity<JobTitle>().HasIndex(j => j.Name).IsUnique();
        modelBuilder.Entity<Employee>().HasIndex(e => e.Email).IsUnique();
        modelBuilder.Entity<Employee>().HasQueryFilter(e => e.IsActive);
        modelBuilder.Entity<Department>().HasQueryFilter(d => d.IsActive);
        modelBuilder.Entity<JobTitle>().HasQueryFilter(j => j.IsActive); 
    } 
}