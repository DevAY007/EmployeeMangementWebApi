using EmployeeManagementLibrary.Dto;
using EmployeeManagementLibrary.Dto;
using EmployeeManagementLibrary.Dto.User;

namespace EmployeeManagementLibrary.Services
{
    public interface IUserService
    {
        //public Task<BaseResponse<IEnumerable<UserDto>>> GetAllUsersAsync();
        //public Task<BaseResponse<bool>> AddUserAsync(AddUserDto request);
        public Task<BaseResponse<UserDto>> GetUserByIdAsync(Guid id);
        public Task<BaseResponse<bool>> UpdateUserAsync(Guid id, UpdateUserDto request);
        public Task<BaseResponse<bool>> DeleteUserAsync(Guid id);
        public Task<BaseResponse<bool>> UserLogin(UserLoginRequestDto request);
        public Task<BaseResponse<Guid>> UserRegistration(AddUserDto request);
        public Task<BaseResponse<bool>> SignOutAsync();

    }
}