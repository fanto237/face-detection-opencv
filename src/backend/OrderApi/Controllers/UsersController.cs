using Microsoft.AspNetCore.Mvc;
using OrderApi.Models;
using OrderApi.Repository;

namespace OrderApi.Controllers;

[ApiController]
[Route("api/v0/usersAuth")]
public class UsersController(IUserRepository repository, ILogger<UsersController> logger) : Controller
{
    private Response? _response;

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Response>> Register([FromBody] UserRegisterDto newUser)
    {

        if (repository.IsUniqueUser(newUser.UserName))
        {
            var user = await repository.Register(newUser);
            _response = new Response("success", user);
            return Ok(_response);
        }
        _response = new Response("error", "User already exists");
            return BadRequest(_response);
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Response>> Login([FromBody] UserLoginRequestDto userLoginRequest)
    {
        var userLoginResponse = await repository.Login(userLoginRequest);

        if (userLoginResponse?.User is null || string.IsNullOrEmpty(userLoginResponse.Token))
        {
            _response = new Response("error", "Invalid credentials");
            return BadRequest(_response);
        }

        _response = new Response("success", userLoginResponse);
        return Ok(_response);
    }


}