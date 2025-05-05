using System.ComponentModel.DataAnnotations;
namespace EmployeeManagementLibrary.Dto.User
{
        public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }


    public class UserLoginRequestDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class UserResetPasswordRequestDto
    {
        public string Email { get; set; }
    }

    public class UserChangePasswordRequestDto
    {
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}