using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tamweely.Application.Interfaces;
using Tamweely.Application.DTOs;

namespace Tamweely.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentRepository _repo;
    public DepartmentsController(IDepartmentRepository repo) { _repo = repo; }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool includeInactive = false) => Ok(await _repo.GetAllAsync(includeInactive));

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DepartmentDto dto) { var id = await _repo.CreateAsync(dto); return CreatedAtAction(nameof(GetAll), new { id }, null); }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] DepartmentDto dto) { await _repo.UpdateAsync(id, dto); return NoContent(); }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) { await _repo.SoftDeleteAsync(id); return NoContent(); }
}