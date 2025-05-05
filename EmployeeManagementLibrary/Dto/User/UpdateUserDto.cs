using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementLibrary.Dto.User
{
     public class UpdateUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }
    }

}