using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using WebApplicationApi.Authorization;
using WebApplicationApi.Enums;
using WebApplicationApi.Models.DataModels;
using WebApplicationApi.Models.Dtos;
using WebApplicationApi.Repositories.Interfaces;

namespace WebApplicationApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private IRepository<UserModel> _repository;

    public AuthController(IRepository<UserModel> repository)
    {
        _repository = repository;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto login)
    {
        var user = await GetUserByCredentials(login);
        if (user == null)
        {
            return Unauthorized();
        }

        var token = new TokenGenerator().GenerateJwtToken((Role)user.RoleId);
        return Ok(
            new
        {
            token, 
            role = (Role)user.RoleId,
            userId = user.Id
        });
    }

    private async Task<UserModel> GetUserByCredentials(LoginDto loginDto)
    {
        var users = await _repository.GetAllAsync();
        
        return users.SingleOrDefault(x => x.Login == loginDto.Username && x.Password == loginDto.Password);
    }
}
