using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tamweely.Application.DTOs;
using Tamweely.Application.Features.Employees;
using Tamweely.Application.Interfaces;

namespace Tamweely.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeRepository _repo;
    private readonly IValidator<CreateOrEditEmployeeDto> _validator;
    public EmployeesController(IEmployeeRepository repo, IValidator<CreateOrEditEmployeeDto> validator)
    {
        _repo = repo;
        _validator = validator;
    }

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
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return BadRequest(new ValidationProblemDetails(
                validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    )));
        }

        try
        {
            var id = await _repo.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }
        catch (DbUpdateException) // Leave unique constraint handling to middleware
        {
            throw;
        }

    }

    [Authorize(Roles = "Admin,User")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateOrEditEmployeeDto dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return BadRequest(new ValidationProblemDetails(
                validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    )));
        }

        try
        {
            await _repo.UpdateAsync(id, dto);
            return NoContent();
        }
        catch (DbUpdateException) // Unique email violation handled in middleware
        {
            throw;
        }
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