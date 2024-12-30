using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OrderApi.Data;
using OrderApi.Models;
namespace OrderApi.Repository;


public class UserRepository(ApplicationDbContext dbContext, IConfiguration config, IMapper mapper) : IUserRepository
{
    public bool IsUniqueUser(string username)
    {
        return !dbContext.Users.Any(u => u.UserName == username);
    }

    public async Task<User> Register(UserRegisterDto user)
    {
        if (IsUniqueUser(user.UserName))
        {
            var userEntity = mapper.Map<User>(user);
            await dbContext.Users.AddAsync(userEntity);
            await dbContext.SaveChangesAsync();
            return userEntity;
        }

        return null;
    }

    public async Task<UserLoginResponseDto> Login(UserLoginRequestDto user)
    {
        var userEntity = await dbContext.Users.FirstOrDefaultAsync(u => u.UserName == user.UserName && u.Password == user.Password);
        if (userEntity == null) return new UserLoginResponseDto(){Token = "", User = null};

        var token = GenerateJwtToken(userEntity);
        var loggedUser = new UserLoginResponseDto(){User = userEntity, Token = token};
        return loggedUser;
    }

    private string GenerateJwtToken(User userEntity)
    {
        var secret = config["Jwt:Secret"];
        var issuer = config["Jwt:Issuer"];
        var key = Encoding.ASCII.GetBytes(secret);
        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userEntity.Id.ToString()),
                new Claim(ClaimTypes.Role, userEntity.Role),
                new Claim(JwtRegisteredClaimNames.Iss, issuer),
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
