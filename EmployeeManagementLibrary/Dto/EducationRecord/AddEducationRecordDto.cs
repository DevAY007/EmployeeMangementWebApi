using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementLibrary.Dto.EducationRecord
{
    public class AddEducationRecordDto
    {
        public Guid employeeId {get; set;}
        public string SchoolName {get; set;}
        public string SchoolType {get; set;}
        public string CourseStudied {get; set;}
        public bool IsGraduated {get; set;}
        public string Certificates {get; set;}
    }
}