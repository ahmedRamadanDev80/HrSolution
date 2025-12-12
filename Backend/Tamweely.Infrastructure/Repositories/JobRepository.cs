using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tamweely.Application.DTOs;
using Tamweely.Application.Interfaces;
using Tamweely.Domain.Entities;
using Tamweely.Infrastructure.Persistence;

namespace Tamweely.Infrastructure.Repositories;

public class JobRepository : IJobRepository
{
    private readonly AppDbContext _db;
    public JobRepository(AppDbContext db) { _db = db; }

    public async Task<IReadOnlyList<JobTitleDto>> GetAllAsync(bool includeInactive = false)
    {
        var q = _db.JobTitles.AsQueryable();
        if (includeInactive) q = q.IgnoreQueryFilters();
        var list = await q.OrderBy(j => j.Name).ToListAsync();
        return list.Select(j => new JobTitleDto { Id = j.Id, Name = j.Name }).ToList();
    }

    public async Task<int> CreateAsync(JobTitleDto dto) 
    {
        var ent = new JobTitle { Name = dto.Name.Trim(), IsActive = true };
        _db.JobTitles.Add(ent);
        await _db.SaveChangesAsync();
        return ent.Id; 
    }
    public async Task UpdateAsync(int id, JobTitleDto dto) 
    {
        var ent = await _db.JobTitles.FindAsync(id);
        if (ent == null) throw new KeyNotFoundException("Job title not found");
        ent.Name = dto.Name.Trim();
        await _db.SaveChangesAsync();
    }
    public async Task SoftDeleteAsync(int id) 
    { 
        var ent = await _db.JobTitles.FindAsync(id);
        if (ent == null) throw new KeyNotFoundException("Job title not found");
        ent.IsActive = false;
        await _db.SaveChangesAsync();
    }
}