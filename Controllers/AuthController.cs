using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TestTask.Dtos.User;
using TestTask.Models;
using TestTask.Repository;
using TestTask.Repository.Interfaces;

namespace TestTask.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;

    public AuthController(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel login)
    {
        var user = _userRepository.FindByUsernameAndPassword(login.Username, login.Password);
        if (user == null) return BadRequest("Invalid credentials.");
        
        var claims = new[] { new Claim(ClaimTypes.Name, login.Username) };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken( _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: creds);
        
        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterModel registerModel)
    {
        var existingUser = _userRepository.FindByUsername(registerModel.Username);
        if (existingUser != null) return BadRequest("Username already exists.");

        var user = new User
        {
            Username = registerModel.Username, 
            Password = registerModel.Password
        };
        _userRepository.Add(user);
        return Ok("User registered successfully.");
    }

}