using alderaan_shop.DTOs.User;
using alderaan_shop.Models;
using alderaan_shop.Models.ClientSided;

namespace alderaan_shop.Services.Interfaces;

public interface IUserService : IService<User>
{
    Task SignupAsync(NewUserDTO newUserDto);
    Task<(string, ClientSidedUser)> LoginAsync(LoginDTO loginDto);
}