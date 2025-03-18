using Models.DTO;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IAuthService
    {
        Task<UserResponseDto> RegisterAsync(UserRegisterDto userDto);
        Task<string> LoginAsync(UserLoginDto loginDto);
    }
}
