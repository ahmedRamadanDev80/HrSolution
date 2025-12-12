using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Tamweely.Application.DTOs;
using Tamweely.Application.Interfaces;
using Tamweely.Domain.Entities;
using Tamweely.Infrastructure.Persistence;
using Tamweely.Infrastructure.Services;

namespace Tamweely.WebAPI.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthController(UserManager<AppUser> userManager, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        var user = await _userManager.FindByNameAsync(req.Username);
        if (user == null || !user.IsActive)
            return Unauthorized("Invalid username or password.");

        if (!await _userManager.CheckPasswordAsync(user, req.Password))
            return Unauthorized("Invalid username or password.");

        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtTokenGenerator.GenerateToken(user, roles);

        return Ok(new { token });
    }

}