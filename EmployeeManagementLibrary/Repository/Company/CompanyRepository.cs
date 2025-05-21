using EmployeeManagementLibrary.AppDbContext;
using EmployeeManagementMVC.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementLibrary.Repository.Company
{
    public class CompanyRepository : ICompanyRepo
    {
		private readonly ApplicationDbContext _dbContext;
			
		public CompanyRepository(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<CompanyRegistration?> GetCompanyByNameAsync (string CompanyName)
		{
			return await _dbContext.Companies.Where(x => x.CompanyName.ToLower() == CompanyName.ToLower()).FirstOrDefaultAsync();
		}

		public async Task<List<CompanyRegistration>> GetAllCompanyAsync()
		{
			return await _dbContext.Companies.ToListAsync();
		}
		public async Task<CompanyRegistration> GetCompanyAsync(Guid id)
		{
			return await _dbContext.Companies.Where(x => x.Id == id).FirstOrDefaultAsync();
		}
	}
}