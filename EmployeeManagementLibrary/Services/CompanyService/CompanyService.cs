using EmployeeManagementLibrary.AppDbContext;
using EmployeeManagementLibrary.Dto;
using EmployeeManagementLibrary.Repository;
using EmployeeManagementLibrary.Dto.Company;
using EmployeeManagementMVC.Models.Entities;
using Microsoft.Extensions.Logging;


namespace EmployeeManagementLibrary.Services.CompanyService
{
    public class CompanyService : ICompanyService
    {
		private readonly ICompanyRepo _companyRepository;
		private readonly ApplicationDbContext _applicationDbContext;
		private readonly ILogger<CompanyService> _logger;

		public CompanyService(ICompanyRepo companyRepository, ApplicationDbContext applicationDbContext, ILogger<CompanyService> logger)
		{
			_companyRepository = companyRepository;
			_applicationDbContext = applicationDbContext;
			_logger = logger;
		}

		public async Task<BaseResponse<Guid>> CreateCompanyAsync(CreateCompanyDto request)
		{
			try
			{
				_logger.LogInformation("Checking for Existing Record");
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

				_logger.LogInformation("Check Complete, Existinng Record not found, Creating new Company");
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
				await _applicationDbContext.Companies.AddAsync(newCompany);
				var saved = await _applicationDbContext.SaveChangesAsync();

				if (saved > 0)
				{
					_logger.LogInformation("Company Created Successfully");
					return new BaseResponse<Guid>
					{
						Data = newCompany.Id,
						Message = "Company created successfully",
						IsSuccessful = true
					};
				}

				_logger.LogError("An Error occured while trying to create Company");
				return new BaseResponse<Guid>
				{
					Message = "Company creation failed",
					IsSuccessful = false
				};
			}
			catch (Exception ex)
			{
				_logger.LogError($"An unexpected Error Occured {ex.Message}");
				return new BaseResponse<Guid>
				{
					Message = $"Error: {ex.Message}",
					IsSuccessful = false
				};
			}
		}


		public async Task<BaseResponse<List<CompanyRegistrationDto>>> GetAllCompanyAsync()
		{
			try
			{
				var company = await _companyRepository.GetAllCompanyAsync();

				if (company.Count > 0)
				{
					_logger.LogInformation("Retriving All Companies Records");
					var data = company.Select(x => new CompanyRegistrationDto
					{
						CompanyId = x.Id,
						CompanyName = x.CompanyName,
						CompanyEmail = x.CompanyEmail,
						About = x.About
					}).ToList();

					_logger.LogInformation("Retrieved all Companies Successfully");
					return new BaseResponse<List<CompanyRegistrationDto>>
					{
						Message = "Record retrieved successfully",
						IsSuccessful = true,
						Data = data
					};
				}

				_logger.LogDebug("Records not Found");
				return new BaseResponse<List<CompanyRegistrationDto>>
				{
					Message = "No record",
					IsSuccessful = false,
					Data = new List<CompanyRegistrationDto>()
				};
			}
			catch (Exception ex)
			{
				_logger.LogError($"An Error Occured {ex.Message}");
				return new BaseResponse<List<CompanyRegistrationDto>>
				{
					Message = $"Error : {ex.Message}",
					IsSuccessful = false,
					Data = new List<CompanyRegistrationDto>()
				};
			}
		}

		public async Task<BaseResponse<CompanyRegistrationDto>> GetCompanyAsync(Guid id)
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

					_logger.LogInformation("Company Records retrieved Successfully");
					return new BaseResponse<CompanyRegistrationDto> 
					{ 
						Message = "Company Record Retrived Successfully", 
						IsSuccessful = true, 
						Data = data 
					};
				}
				_logger.LogDebug("No Record Found");
				return new BaseResponse<CompanyRegistrationDto> 
				{ 
					Message = "No Record ", 
					IsSuccessful = false, 
					Data = new CompanyRegistrationDto() 
				};

			}
			catch (Exception ex)
			{
				_logger.LogDebug($"An error occured{ex.Message}");
				return new BaseResponse<CompanyRegistrationDto> 
				{ 
					Message = $"Error : {ex.Message}", 
					IsSuccessful = false, 
					Data = new CompanyRegistrationDto() 
				};
			}
		}

		public async Task<BaseResponse<bool>> UpdateCompanyAsync(Guid id, UpdateCompanyDto request)
		{
			try
			{
				_logger.LogInformation("Update Company Method Called");
				var company = await _companyRepository.GetCompanyAsync(id);
				if (company != null)
				{
					company.CompanyName = request.CompanyName;
					company.CompanyEmail = request.CompanyEmail;
					company.About = request.About;

					_applicationDbContext.Companies.Update(company);

					if (await _applicationDbContext.SaveChangesAsync() > 0)
					{
						_logger.LogInformation("Company Records Updated");
						return new BaseResponse<bool> 
						{ 
							Message = "Company Record updated successfully", 
							IsSuccessful = true, 
							Data = true 
						};
					}
				}

				-logger.LogError("Company Record cannot be found");
				return new BaseResponse<bool> 
				{ 
					Message = "Record not found", 
					IsSuccessful = false, 
					Data = false 
				};
			}
			catch (Exception ex)
			{
				-logger.LogDebug($"An Error Occured {ex.Message}")
				return new BaseResponse<bool> 
				{ Message = $"Error :  {ex.Message}", 
				IsSuccessful = false, 
				Data = false 
			};
			}
		}

		public async Task<BaseResponse<bool>> DeleteCompanyAsync(Guid id)
		{
			try
			{
				_logger.LogInformation("Delete Method Called");
				var company = await _companyRepository.GetCompanyAsync(id);

				if (company != null)
				{
					_applicationDbContext.Companies.Remove(company);

					if (await _applicationDbContext.SaveChangesAsync() > 0)
					{
						_logger.LogInformation("Company Deleted Successfully");
						return new BaseResponse<bool> { Message = "Company Record Deleted Successfully", IsSuccessful = true, Data = true };
					}
				}
				_logger.LogDebug("Company Record Not Found Failed to delete Company");
				return new BaseResponse<bool> { Message = "Recprd not found", IsSuccessful = false, Data = false };
				
			}
			catch (Exception ex)
			{
				_logger.LogError($"An Error Occured {ex.Message}");
				return new BaseResponse<bool> { Message = $"Error :  {ex.Message}", IsSuccessful = false, Data = false };
			}
		}
    }
}