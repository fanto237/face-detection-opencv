using OrderApi.Models;

namespace OrderApi.Repository;

public interface IUserRepository
{
    bool IsUniqueUser(string username);
    Task<User> Register(UserRegisterDto user);
    Task<UserLoginResponseDto?> Login(UserLoginRequestDto user);
}


