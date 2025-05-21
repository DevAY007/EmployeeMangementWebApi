using EmployeeManagementLibrary.AppDbContext;
using EmployeeManagementMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementLibrary.Repository.EmployeeRepo
{
    public class EmployeeRepository : IEmployeeRepository
    {
         private readonly ApplicationDbContext _dbContext;

        public EmployeeRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Employee>> GetAllEmployeeAsync()
        {
            return await _dbContext.Emlpoyees.ToListAsync();
        }

        public async Task<Employee> GetEmployeeAsync(Guid id)
        {
            return await _dbContext.Emlpoyees.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Employee> GetCompanyByEmployeeIdAsync(Guid employeeId)
        {
            return await _dbContext.Emlpoyees
                .Where(x => x.Id == employeeId)
                .FirstOrDefaultAsync();
        }



        public async Task<Employee?> GetEmployeeByFirstNameAsync(string FirstName)
        {
            return await _dbContext.Emlpoyees.Where(x => x.FirstName.ToLower() == FirstName.ToLower()).FirstOrDefaultAsync();
        }

        public async Task<List<Employee>> GetEmployeesByCompanyIdAsync(Guid companyId)
        {
            return await _dbContext.Emlpoyees.Where(e => e.CompanyId == companyId).ToListAsync();
        }
    }
}