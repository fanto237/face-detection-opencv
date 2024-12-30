namespace OrderApi.Models;

public record User(Guid Id, string Name, string UserName, string Password, string Role);