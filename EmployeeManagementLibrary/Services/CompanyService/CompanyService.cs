using EmployeeManagementLibrary.AppDbContext;
using EmployeeManagementLibrary.Dto;
using EmployeeManagementLibrary.Repository;
using EmployeeManagementLibrary.Dto.Company;
using EmployeeManagementMVC.Models.Entities;

namespace EmployeeManagementLibrary.Services.CompanyService
{
    public class CompanyService : ICompanyService
    {
		private readonly ICompanyRepo _companyRepository;
		private readonly ApplicationDbContext _applicationDbContext;

		public CompanyService(ICompanyRepo companyRepository, ApplicationDbContext applicationDbContext)
		{
			_companyRepository = companyRepository;
			_applicationDbContext = applicationDbContext;
		}

		public async Task<BaseResponse<Guid>> CreateCompany(CreateCompanyDto request)
		{
			try
			{
				// Check if the company already exists
				var existingCompany = await _companyRepository.GetCompanyByNameAsync(request.CompanyName);
				if (existingCompany != null)
				{
					return new BaseResponse<Guid>
					{
						Message = "Company record already exists",
						IsSuccessful = false
					};
				}

				// Create new company entity
				var newCompany = new CompanyRegistration
				{
					Id = Guid.NewGuid(),
					UserId = request.UserId,
					CompanyName = request.CompanyName,
					CompanyEmail = request.CompanyEmail,
					About = request.About
				};

				// Save to database
				await _applicationDbContext.Company.AddAsync(newCompany);
				var saved = await _applicationDbContext.SaveChangesAsync();

				if (saved > 0)
				{
					return new BaseResponse<Guid>
					{
						Data = newCompany.Id,
						Message = "Company created successfully",
						IsSuccessful = true
					};
				}

				return new BaseResponse<Guid>
				{
					Message = "Company creation failed",
					IsSuccessful = false
				};
			}
			catch (Exception ex)
			{
				return new BaseResponse<Guid>
				{
					Message = $"Error: {ex.Message}",
					IsSuccessful = false
				};
			}
		}


		public async Task<BaseResponse<List<CompanyRegistrationDto>>> GetAllCompany()
		{
			try
			{
				var company = await _companyRepository.GetAllCompanyAsync();

				if (company.Count > 0)
				{
					var data = company.Select(x => new CompanyRegistrationDto
					{
						CompanyId = x.Id,
						CompanyName = x.CompanyName,
						CompanyEmail = x.CompanyEmail,
						About = x.About
					}).ToList();
					return new BaseResponse<List<CompanyRegistrationDto>> { Message = "Record retrieved successfully", IsSuccessful = true, Data = data };
				}
				return new BaseResponse<List<CompanyRegistrationDto>> { Message = "No record", IsSuccessful = false, Data = new List<CompanyRegistrationDto>() };
			}
			catch (Exception ex)
			{
				return new BaseResponse<List<CompanyRegistrationDto>> { Message = $"Error : {ex.Message}", IsSuccessful = false, Data = new List<CompanyRegistrationDto>() };
			}
		}

		public async Task<BaseResponse<CompanyRegistrationDto>> GetCompany(Guid id)
		{
			try
			{
				var company = await _companyRepository.GetCompanyAsync(id);
				if (company != null)
				{
					var data = new CompanyRegistrationDto
					{
						CompanyId = company.Id,
						CompanyName = company.CompanyName,
						CompanyEmail = company.CompanyEmail,
						About = company.About,
					};
					return new BaseResponse<CompanyRegistrationDto> { Message = "Company Record Retrived Successfully", IsSuccessful = true, Data = data };
				}
				return new BaseResponse<CompanyRegistrationDto> { Message = "No Record ", IsSuccessful = false, Data = new CompanyRegistrationDto() };

			}
			catch (Exception ex)
			{
				return new BaseResponse<CompanyRegistrationDto> { Message = $"Error : {ex.Message}", IsSuccessful = false, Data = new CompanyRegistrationDto() };
			}
		}

		public async Task<BaseResponse<bool>> UpdateCompany(Guid id, UpdateCompanyDto request)
		{
			try
			{
				var company = await _companyRepository.GetCompanyAsync(id);
				if (company != null)
				{
					company.CompanyName = request.CompanyName;
					company.CompanyEmail = request.CompanyEmail;
					company.About = request.About;

					_applicationDbContext.Company.Update(company);

					if (await _applicationDbContext.SaveChangesAsync() > 0)
					{
						return new BaseResponse<bool> { Message = "Company Record updated successfully", IsSuccessful = true, Data = true };
					}
				}

				return new BaseResponse<bool> { Message = "Record not found", IsSuccessful = false, Data = false };
			}
			catch (Exception ex)
			{
				return new BaseResponse<bool> { Message = $"Error :  {ex.Message}", IsSuccessful = false, Data = false };
			}
		}

		public async Task<BaseResponse<bool>> Delete(Guid id)
		{
			try
			{
				var company = await _companyRepository.GetCompanyAsync(id);

				if (company != null)
				{
					_applicationDbContext.Company.Remove(company);

					if (await _applicationDbContext.SaveChangesAsync() > 0)
					{
						return new BaseResponse<bool> { Message = "Company Record Deleted Successfully", IsSuccessful = true, Data = true };
					}
				}
				return new BaseResponse<bool> { Message = "Book not found", IsSuccessful = false, Data = false };
				
			}
			catch (Exception ex)
			{
				return new BaseResponse<bool> { Message = $"Error :  {ex.Message}", IsSuccessful = false, Data = false };
			}
		}
    }
}