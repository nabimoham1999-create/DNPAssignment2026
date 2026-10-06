using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository userRepo;

    public UserController(IUserRepository userRepo)
    {
        this.userRepo = userRepo;
    }

    [HttpGet]
    public ActionResult<IEnumerable<UserDto>> GetUsers(
        [FromQuery] string? userName)
    {
        IQueryable<User> query = userRepo.GetMany();

        if (!string.IsNullOrWhiteSpace(userName))
        {
            query = query.Where(user =>
                user.UserName.Contains(
                    userName,
                    StringComparison.OrdinalIgnoreCase));
        }

        IEnumerable<UserDto> users = query
            .Select(user => new UserDto
            {
                Id = user.Id,
                UserName = user.UserName
            });

        return Ok(users);
    }
    
    [HttpGet("{id:int}")]
    public ActionResult<UserDto> GetUser(int id)
    {
        User? user = userRepo
            .GetMany()
            .FirstOrDefault(u => u.Id == id);

        if (user == null)
        {
            return NotFound();
        }

        UserDto dto = new UserDto
        {
            Id = user.Id,
            UserName = user.UserName
        };
        
        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto dto)
    {
        User user = new User
        {
            UserName = dto.UserName,
            Password = dto.Password
        };
        
        User createdUser = await userRepo.AddAsync(user);

        UserDto userDto = new UserDto
        {
            Id = createdUser.Id,
            UserName = createdUser.UserName
        };
        
        return CreatedAtAction(
            nameof(GetUser),
            new {
                id = createdUser.Id },  
                userDto);
        
    }
    
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateUser(int id, CreateUserDto dto)
    {
        User user = new User
        {
            Id = id,
            UserName = dto.UserName,
            Password = dto.Password
        };

        await userRepo.UpdateAsync(user);

        return NoContent();
    }
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        await userRepo.DeleteAsync(id);

        return NoContent();
    }
}