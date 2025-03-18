using Constants;
using Models.DTO;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IUserService
    {
        Task<UserResponseDto> GetUserByIdAsync(int userId);
        Task ChangeUserRoleAsync(int userId, RoleEnum newRole);
    }
}
