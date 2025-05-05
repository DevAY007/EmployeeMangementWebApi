using EmployeeManagementMVC.Models.Entities;

namespace EmployeeManagementLibrary.Repository
{
    public interface ICompanyRepo
    {
    	Task<CompanyRegistration?> GetCompanyByNameAsync(string CompanyName);
	    Task<List<CompanyRegistration>> GetAllCompanyAsync();
	    Task<CompanyRegistration> GetCompanyAsync(Guid id);
    }
}