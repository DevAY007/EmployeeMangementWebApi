using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementLibrary.Dto.Company
{
    public class CompanyRegistrationDto
    {
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyEmail { get; set; }
        public string About { get; set; }
    }
}