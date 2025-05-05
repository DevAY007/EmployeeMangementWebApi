using EmployeeManagementMVC.Models;

namespace EmployeeManagementLibrary.Repository.EmployeeRepo
{
    public interface IEmployeeRepository
    {
         Task<Employee> GetEmployeeAsync(Guid id);
        Task<Employee?> GetEmployeeByFirstNameAsync(string FirstName);
        Task<List<Employee>> GetAllEmployeeAsync();
        Task<List<Employee>> GetEmployeesByCompanyIdAsync(Guid companyId);
        Task<Employee> GetCompanyByEmployeeIdAsync(Guid employeeId);
    }
}