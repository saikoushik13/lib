using AutoMapper;
using Constants;
using DatabaseOperations.Interface;
using Models.DTO;
using Service.Interface;
using System;
using System.Threading.Tasks;

namespace Service
{
    public class UserService : IUserService
    {
        private readonly IUserDatabaseOperations _userDbOperations;
        private readonly IMapper _mapper;

        public UserService(IUserDatabaseOperations userDbOperations, IMapper mapper)
        {
            _userDbOperations = userDbOperations;
            _mapper = mapper;
        }

        public async Task<UserResponseDto> GetUserByIdAsync(int userId)
        {
            var user = await _userDbOperations.GetByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found.");

            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task ChangeUserRoleAsync(int userId, RoleEnum newRole)
        {
            var user = await _userDbOperations.GetByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found.");

            user.Role = newRole;
            await _userDbOperations.SaveChangesAsync();
        }
    }
}
