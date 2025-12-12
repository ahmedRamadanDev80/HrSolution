using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tamweely.Application.Features.Employees;
using Tamweely.Application.Interfaces;
using Tamweely.Application.DTOs;

namespace Tamweely.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeRepository _repo;
    public EmployeesController(IEmployeeRepository repo) { _repo = repo; }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] EmployeeQueryParams q)
    {
        var (items, total) = await _repo.SearchAsync(q);
        return Ok(new { items, total, page = q.Page, pageSize = q.PageSize });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var e = await _repo.GetByIdAsync(id);
        if (e == null) return NotFound();
        return Ok(e);
    }

    [Authorize(Roles = "Admin,User")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrEditEmployeeDto dto)
    {
        var id = await _repo.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    [Authorize(Roles = "Admin,User")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateOrEditEmployeeDto dto)
    {
        await _repo.UpdateAsync(id, dto);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repo.SoftDeleteAsync(id);
        return NoContent();
    }

    [HttpGet("export")]
    public async Task<IActionResult> Export([FromQuery] EmployeeQueryParams q)
    {
        var bytes = await _repo.ExportToExcelAsync(q);
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"employees_{DateTime.Now:yyyyMMddHHmm}.xlsx");
    }
}