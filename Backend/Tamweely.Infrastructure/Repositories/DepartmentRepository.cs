using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tamweely.Application.DTOs;
using Tamweely.Application.Interfaces;
using Tamweely.Domain.Entities;
using Tamweely.Infrastructure.Persistence;

namespace Tamweely.Infrastructure.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly AppDbContext _db;
    public DepartmentRepository(AppDbContext db) { _db = db; }

    public async Task<IReadOnlyList<DepartmentDto>> GetAllAsync(bool includeInactive = false)
    {
        var q = _db.Departments.AsQueryable();
        if (includeInactive) q = q.IgnoreQueryFilters();
        var list = await q.OrderBy(d => d.Name).ToListAsync();
        return list.Select(d => new DepartmentDto { Id = d.Id, Name = d.Name }).ToList();
    }
    public async Task<int> CreateAsync(DepartmentDto dto) 
    {
        var ent = new Department { Name = dto.Name.Trim(), IsActive = true };
        _db.Departments.Add(ent);
        await _db.SaveChangesAsync();
        return ent.Id; 
    }
    public async Task UpdateAsync(int id, DepartmentDto dto) 
    {
        var ent = await _db.Departments.FindAsync(id);
        if (ent == null) throw new KeyNotFoundException("Department not found");
        ent.Name = dto.Name.Trim();
        await _db.SaveChangesAsync();
    }
    public async Task SoftDeleteAsync(int id) 
    {
        var ent = await _db.Departments.FindAsync(id);
        if (ent == null) throw new KeyNotFoundException("Department not found");
        ent.IsActive = false; 
        await _db.SaveChangesAsync(); 
    }
}