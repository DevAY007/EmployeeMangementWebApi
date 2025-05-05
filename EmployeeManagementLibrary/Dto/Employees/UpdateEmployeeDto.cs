using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementLibrary.Dto.Employees
{
    public class UpdateEmployeeDto
    {
        public Guid Id { get; set; }
		public Guid CompanyId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string DOB { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public string HomeAddress { get; set; }
		public string MaritalStatus { get; set; }
    }
}