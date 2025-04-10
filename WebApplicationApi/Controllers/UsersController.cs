using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using WebApplicationApi.Authentication;
using WebApplicationApi.Enums;
using WebApplicationApi.Models.DataModels;
using WebApplicationApi.Models.Dtos.User;
using WebApplicationApi.Repositories.Interfaces;

namespace WebApplicationApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = $"{nameof(Role.Customer)},{nameof(Role.Admin)}")]
[ServiceFilter(typeof(ApiKeyAuthFilter))]
public class UsersController : ControllerBase
{
    private IRepository<UserModel> _repository;
    public UsersController(IRepository<UserModel> repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserModel>>> GetUsers()
    {
        var users = await _repository.GetAllAsync();
        return Ok(users);
    }

    [HttpPost]
    public async Task<ActionResult<UserModel>> CreateUser(UserDto addUserDto)
    {
        var user = new UserModel()
        {
            Name = addUserDto.Name,
            Email = addUserDto.Email,
            RoleId = (int)addUserDto.Role,
            Phone = addUserDto.Phone,
            Address = addUserDto.Address,
            Login = addUserDto.Login,
            Password = addUserDto.Password
        };

        var createdUser = await _repository.AddAsync(user);

        return CreatedAtAction(nameof(CreateUser), new { id = createdUser.Id }, createdUser);

    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<UserModel>> GetUser(int id)
    {
        var user = await _repository.GetByIdAsync(id);

        if (user == null)
        {
            return NotFound(new { message = "User was not found" });
        }

        return Ok(user);
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<ActionResult> UpdateUser(int id, UserDto userDto)
    {
        var user = new UserModel()
        {
            Name = userDto.Name,
            RoleId = (int)userDto.Role,
            Phone = userDto.Phone,
            Login = userDto.Login,
            Password = userDto.Password,
            Email = userDto.Email,
            Address = userDto.Address,
        };

        var result = await _repository.UpdateAsync(id, user);

        if (result == null)
        {
            return NotFound(new { message = "User was not found" });
        }

        return Ok(result);
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        var isDeleted = await _repository.DeleteAsync(id);

        if (!isDeleted)
        {
            return NotFound(new { message = "User was not found" });
        }

        return Ok();
    }
}
