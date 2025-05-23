using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementLibrary.Dto.Company
{
    public class CreateCompanyDto
    {
		public Guid UserId {get; set;}

		[Required(ErrorMessage = "Company Name Is required")]
		public string CompanyName { get; set; }

		[Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string CompanyEmail { get; set; }
		
		public string About { get; set; }
    }
}