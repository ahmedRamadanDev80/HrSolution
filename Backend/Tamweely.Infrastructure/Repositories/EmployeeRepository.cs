using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tamweely.Application.DTOs;
using Tamweely.Application.Features.Employees;
using Tamweely.Application.Interfaces;
using Tamweely.Domain.Entities;
using ClosedXML.Excel;
using Tamweely.Infrastructure.Persistence;

namespace Tamweely.Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext _db;

    public EmployeeRepository(AppDbContext db) { _db = db; }

    public async Task<(IReadOnlyList<EmployeeDto> Items, int Total)> SearchAsync(EmployeeQueryParams q)
    {
        var query = _db.Employees.Include(e => e.Department).Include(e => e.JobTitle).AsQueryable();

        if (!string.IsNullOrWhiteSpace(q.Search))
        {
            var s = q.Search.Trim().ToLower();
            query = query.Where(e => e.FirstName.ToLower().Contains(s) || e.LastName.ToLower().Contains(s) || e.Email.ToLower().Contains(s) || e.Mobile.Contains(s));
        }

        if (q.DepartmentId.HasValue) query = query.Where(e => e.DepartmentId == q.DepartmentId.Value);
        if (q.JobTitleId.HasValue) query = query.Where(e => e.JobTitleId == q.JobTitleId.Value);
        if (q.DobFrom.HasValue) query = query.Where(e => e.DateOfBirth >= q.DobFrom.Value);
        if (q.DobTo.HasValue) query = query.Where(e => e.DateOfBirth <= q.DobTo.Value);

        var total = await query.CountAsync();
        var items = await query.OrderBy(e => e.Id).Skip((q.Page - 1) * q.PageSize).Take(q.PageSize)
            .Select(e => new EmployeeDto {
                Id = e.Id,
                FullName = e.FirstName + " " + e.LastName,
                Email = e.Email,
                Mobile = e.Mobile,
                DateOfBirth = e.DateOfBirth,
                DepartmentId = e.DepartmentId,
                DepartmentName = e.Department != null ? e.Department.Name : string.Empty,
                JobTitleId = e.JobTitleId,
                JobTitleName = e.JobTitle != null ? e.JobTitle.Name : string.Empty
            }).ToListAsync();

        return (items, total);
    }

    public async Task<EmployeeDto?> GetByIdAsync(int id)
    {
        var e = await _db.Employees.Include(x => x.Department).Include(x => x.JobTitle).FirstOrDefaultAsync(x => x.Id == id);
        if (e == null) return null;
        return new EmployeeDto {
            Id = e.Id, FullName = e.FullName, Email = e.Email, Mobile = e.Mobile, DateOfBirth = e.DateOfBirth,
            DepartmentId = e.DepartmentId, DepartmentName = e.Department?.Name ?? string.Empty,
            JobTitleId = e.JobTitleId, JobTitleName = e.JobTitle?.Name ?? string.Empty
        };
    }

    public async Task<int> CreateAsync(CreateOrEditEmployeeDto dto)
    {
        var ent = new Employee {
            FirstName = dto.FirstName.Trim(),
            LastName = dto.LastName.Trim(),
            Email = dto.Email.Trim().ToLower(),
            Mobile = dto.Mobile.Trim(),
            DateOfBirth = dto.DateOfBirth,
            DepartmentId = dto.DepartmentId,
            JobTitleId = dto.JobTitleId,
            IsActive = true
        };
        _db.Employees.Add(ent);
        await _db.SaveChangesAsync();
        return ent.Id;
    }

    public async Task UpdateAsync(int id, CreateOrEditEmployeeDto dto)
    {
        var ent = await _db.Employees.FindAsync(id);
        if (ent == null) throw new KeyNotFoundException("Employee not found");
        ent.FirstName = dto.FirstName.Trim(); ent.LastName = dto.LastName.Trim();
        ent.Email = dto.Email.Trim().ToLower(); ent.Mobile = dto.Mobile.Trim();
        ent.DateOfBirth = dto.DateOfBirth; ent.DepartmentId = dto.DepartmentId; ent.JobTitleId = dto.JobTitleId;
        await _db.SaveChangesAsync();
    }

    public async Task SoftDeleteAsync(int id)
    {
        var ent = await _db.Employees.FindAsync(id);
        if (ent == null) throw new KeyNotFoundException("Employee not found");
        ent.IsActive = false;
        await _db.SaveChangesAsync();
    }

    public async Task<byte[]> ExportToExcelAsync(EmployeeQueryParams q)
    {
        var query = _db.Employees.Include(e => e.Department).Include(e => e.JobTitle).AsQueryable();
        if (!string.IsNullOrWhiteSpace(q.Search)) { var s = q.Search.Trim().ToLower(); query = query.Where(e => e.FirstName.ToLower().Contains(s) || e.LastName.ToLower().Contains(s) || e.Email.ToLower().Contains(s) || e.Mobile.Contains(s)); }
        if (q.DepartmentId.HasValue) query = query.Where(e => e.DepartmentId == q.DepartmentId.Value);
        if (q.JobTitleId.HasValue) query = query.Where(e => e.JobTitleId == q.JobTitleId.Value);
        if (q.DobFrom.HasValue) query = query.Where(e => e.DateOfBirth >= q.DobFrom.Value);
        if (q.DobTo.HasValue) query = query.Where(e => e.DateOfBirth <= q.DobTo.Value);

        var list = await query.OrderBy(e => e.Id).Select(e => new EmployeeDto {
            Id = e.Id,
            FullName = e.FirstName + " " + e.LastName,
            Email = e.Email,
            Mobile = e.Mobile,
            DateOfBirth = e.DateOfBirth,
            DepartmentName = e.Department != null ? e.Department.Name : string.Empty,
            JobTitleName = e.JobTitle != null ? e.JobTitle.Name : string.Empty
        }).ToListAsync();

        using var wb = new XLWorkbook();
        var ws = wb.AddWorksheet("Employees");
        ws.Cell(1,1).Value = "Id"; ws.Cell(1,2).Value = "FullName"; ws.Cell(1,3).Value = "Email"; ws.Cell(1,4).Value = "Mobile"; ws.Cell(1,5).Value = "DateOfBirth"; ws.Cell(1,6).Value = "Department"; ws.Cell(1,7).Value = "JobTitle";
        int r = 2;
        foreach (var e in list) { ws.Cell(r,1).Value = e.Id; ws.Cell(r,2).Value = e.FullName; ws.Cell(r,3).Value = e.Email; ws.Cell(r,4).Value = e.Mobile; ws.Cell(r,5).Value = e.DateOfBirth?.ToString("yyyy-MM-dd"); ws.Cell(r,6).Value = e.DepartmentName; ws.Cell(r,7).Value = e.JobTitleName; r++; }
        using var ms = new System.IO.MemoryStream(); wb.SaveAs(ms); return ms.ToArray();
    }
}