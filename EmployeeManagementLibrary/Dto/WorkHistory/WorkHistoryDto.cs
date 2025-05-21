using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementLibrary.Dto.WorkHistory
{
    public class WorkHistoryDto
    {
        public Guid Id {get; set;}
        public Guid employeeId {get; set;}
        public string CompanyName {get; set;}
        public string Responsibility {get; set;}
        public DateTimeOffset StartDate { get; set;}
        public DateTimeOffset EndDate {get; set;}
        public bool IsCurrentJob {get; set;}
    }
}