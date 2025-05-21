using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagementMVC.Models;

namespace EmployeeManagementLibrary.Dto.EducationRecord
{
    public class EducationRecordDto
    {
        public Guid Id {get; set;}
        public Guid employeeId {get; set;}
        public string SchoolName {get; set;}
        public string SchoolType {get; set;}
        public string CourseStudied {get; set;}
        public string Certificates {get; set;}
        public bool IsGraduated {get; set;}

        public Employee Employee {get; set;}
    }
}