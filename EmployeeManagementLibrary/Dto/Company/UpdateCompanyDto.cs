using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementLibrary.Dto.Company
{
    public class UpdateCompanyDto
    {
        public string CompanyName { get; set; }

        [EmailAddress]
		public string CompanyEmail { get; set; }

		public string About { get; set; }
    }
}