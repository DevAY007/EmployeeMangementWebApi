using EmployeeManagementMVC.Models;
using EmployeeManagementMVC.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace EmployeeManagementLibrary.AppDbContext
{
     public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Employee> Emlpoyees { get; set; }
		public DbSet<WorkHistory> WorkHistories { get; set; }
		public DbSet<MedicalStatus> MedicalStatuses{ get; set; }
        public DbSet<EducationHistory> EducationRecords {get; set;}
		public DbSet<CompanyRegistration> Companies { get; set; }
    }
}