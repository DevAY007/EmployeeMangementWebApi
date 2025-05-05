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

        public DbSet<Employee> emlpoyees { get; set; }
		public DbSet<NextOfKin> NextKin { get; set; }
		public DbSet<EducationHistory> EduInformation { get; set; }
		public DbSet<HealthStatus> HealthStatus{ get; set; }
		public DbSet<CompanyRegistration> Company { get; set; }
    }
}