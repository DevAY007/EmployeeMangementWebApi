using EmployeeManagementLibrary.AppDbContext;
using EmployeeManagementLibrary.Dto;
using EmployeeManagementLibrary.Dto.Employees;
using EmployeeManagementLibrary.Repository.EmployeeRepo;
using EmployeeManagementMVC.Models;
using Microsoft.Extensions.Logging;


namespace EmployeeManagementLibrary.Services.EmployeeServices
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
		private readonly ApplicationDbContext _dbContext;
		private readonly ILogger<EmployeeService> _logger;

		public EmployeeService(IEmployeeRepository employeeRepository, ApplicationDbContext dbContext, ILogger<EmployeeService> logger)
		{
			_employeeRepository = employeeRepository;
			_dbContext = dbContext;
			_logger = logger;
		}


		public async Task<BaseResponse<Guid>> AddEmployeeAsync(AddEmployeeDto request)
		{
			try
			{
				var employee = await _employeeRepository.GetEmployeeByFirstNameAsync(request.FirstName);
				if (employee != null)
				{
					_logger.LogInformation("Employee Record Already Exist");
					return new BaseResponse<Guid>
					{
						Message = "Employee Record Already Exist",
						IsSuccessful = false,
						Data = Guid.Empty 
					};
				}

				_logger.LogInformation("Creating new Employee");
				var newEmployee = new Employee()
				{
					Id = Guid.NewGuid(),
					CompanyId = request.CompanyId,
					FirstName = request.FirstName,
					LastName = request.LastName,
					DOB = request.DOB,
					Gender = request.Gender,
					Email = request.Email,
					PhoneNumber = request.PhoneNumber,
					HomeAddress = request.HomeAddress,
					MaritalStatus = request.MaritalStatus
				};

				await _dbContext.Emlpoyees.AddAsync(newEmployee);
				if (await _dbContext.SaveChangesAsync() > 0)
				{
					_logger.LogInformation("Employee records added successfully");
					return new BaseResponse<Guid>
					{
						Message = "Employee added successfully",
						IsSuccessful = true,
						Data = newEmployee.Id
					};
				}

				_logger.LogError("Failed to add employee records");
				return new BaseResponse<Guid>
				{
					Message = "Failed To Create Employee",
					IsSuccessful = false,
					Data = Guid.Empty
				};
			}
			catch (Exception ex)
			{
				_logger.LogDebug($"An error occured {ex.Message}");
				return new BaseResponse<Guid>
				{
					Message = $"Error: {ex.Message}",
					IsSuccessful = false,
					Data = Guid.Empty
				};
			}
		}

		//public async Task<BaseResponse<List<EmployeeDto>>> GetAllEmployee()
		//{
		//	try 
		//	{
		//		var employee = await _employeeRepository.GetAllEmployeeAsync();

		//		if (employee.Count > 0)
		//		{
		//			var data = employee.Select(x => new EmployeeDto
		//			{
		//				Id = x.Id,
		//				CompanyId = x.CompanyId,
		//				FirstName = x.FirstName,
		//				LastName = x.LastName,
		//				DOB = x.DOB,
		//				Gender = x.Gender,
		//				Email = x.Email,
		//				PhoneNumber = x.PhoneNumber,
		//				HomeAddress = x.HomeAddress,
		//				MaritalStatus = x.MaritalStatus
		//			}).ToList();
		//			return new BaseResponse<List<EmployeeDto>> 
		//			{ 
		//				Message = "Record retrieved successfully", 
		//				IsSuccessful = true, 
		//				Data = data 
		//			};
		//		}
		//		return new BaseResponse<List<EmployeeDto>> 
		//		{ 
		//			Message = "No record", 
		//			IsSuccessful = false, 
		//			Data = new List<EmployeeDto>() 
		//		};
		//	}
		//	catch (Exception ex)
		//	{
		//		return new BaseResponse<List<EmployeeDto>> 
		//		{ Message = $"Error : {ex.Message}", 
		//			IsSuccessful = false, 
		//			Data = new List<EmployeeDto>() 
		//		};
		//	}
		//}

		public async Task<BaseResponse<EmployeeDto>> GetEmployeeAsync(Guid id)
		{
			try
			{
				_logger.LogInformation("Get employee method called");
				var employee = await _employeeRepository.GetEmployeeAsync(id);
				if (employee != null)
				{
					_logger.LogInformation("Retrieving employee records");
					var data = new EmployeeDto
					{
						employeeId = employee.Id,
						CompanyId = employee.CompanyId,
						FirstName = employee.FirstName,
						LastName = employee.LastName,
						DOB = employee.DOB,
						Gender = employee.Gender,
						Email = employee.Email,
						PhoneNumber = employee.PhoneNumber,
						HomeAddress = employee.HomeAddress,
						MaritalStatus = employee.MaritalStatus,
					};

					_logger.LogInformation("Recovered employee records successfully");
					return new BaseResponse<EmployeeDto>
					{
						Message = "Record Retrieved Successfully",
						IsSuccessful = true,
						Data = data
					};
				}

				_logger.LogDebug("Failed to retrieve employee records");
				return new BaseResponse<EmployeeDto>
				{
					Message = "No Record Found",
					IsSuccessful = false,
					Data = null
				};
			}
			catch (Exception ex)
			{
				_logger.LogError($"{ex.Message}");
				return new BaseResponse<EmployeeDto>
				{
					Message = $"Error: {ex.Message}",
					IsSuccessful = false,
					Data = null
				};
			}
		}

		public async Task<BaseResponse<Guid>> GetCompanyIdByEmployeeIdAsync(Guid employeeId)
        {
            try
            {
				_logger.LogInformation("Get Company By EmployeeId method called");
                var employee = await _employeeRepository.GetCompanyByEmployeeIdAsync(employeeId);

                if (employee != null)
                {
					_logger.LogInformation("Retrieved Company ID successfully");
                    return new BaseResponse<Guid>
					{
						Message = "Company ID Retrieved Successfully",
						IsSuccessful = true,
						Data = employee.CompanyId
					};
                }

				_logger.LogInformation("Company Id Cannot be found");
                return new BaseResponse<Guid>
				{
					Message = "No Record Found",
					IsSuccessful = false,
					Data = Guid.Empty
				};
            }
            catch (Exception ex)
            {
				_logger.LogError($"{ex.Message}");
                return new BaseResponse<Guid>
				{
					Message = $"Error: {ex.Message}",
					IsSuccessful = false,
					Data = Guid.Empty
				};
            }
        }


        public async Task<BaseResponse<List<EmployeeDto>>> GetAllEmployeesByCompanyIdAsync(Guid companyId)
        {
            try
            {
				_logger.LogInformation("Get all Employees by Company ID method called");
                var employees = await _employeeRepository.GetEmployeesByCompanyIdAsync(companyId);

                if (employees != null && employees.Count > 0)
                {
					_logger.LogInformation("Retrieving all employee records for {companyId}",companyId);
                    var employeeDtos = employees.Select(e => new EmployeeDto
                    {
                        employeeId = e.Id,
                        CompanyId = e.CompanyId,
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        DOB = e.DOB,
                        Gender = e.Gender,
                        Email = e.Email,
                        PhoneNumber = e.PhoneNumber,
						HomeAddress = e.HomeAddress,
                        MaritalStatus = e.MaritalStatus
                    }).ToList();

					_logger.LogInformation("Employees record for {companyId} retrieved successfully", companyId);
                    return new BaseResponse<List<EmployeeDto>>
					{
						Message = "Employees retrieved successfully",
						IsSuccessful = true,
						Data = employeeDtos
					};
                }

				_logger.LogInformation("Can't find any employee for {companyId}", companyId);
                return new BaseResponse<List<EmployeeDto>>
				{
					Message = "No employees found for this company.",
					IsSuccessful = false,
					Data = new List<EmployeeDto>()
				};
            }
            catch (Exception ex)
            {
				_logger.LogError(ex, "An error occurred while processing the request.");
                return new BaseResponse<List<EmployeeDto>>
				{
					Message = $"Error: {ex.Message}",
					IsSuccessful = false,
					Data = new List<EmployeeDto>()
				};
            }
        }

        public async Task<BaseResponse<Guid>> UpdateEmployeeAsync(Guid id, UpdateEmployeeDto request, Guid companyId)
		{
			try
			{
				_logger.LogInformation("Update employee method called");
				var employee = await _employeeRepository.GetEmployeeAsync(id);
				
				if (employee != null)
				{
					_logger.LogInformation("Updating Employee Records");
					employee.FirstName = request.FirstName;
					employee.LastName = request.LastName;
					employee.DOB = request.DOB;
					employee.Gender = request.Gender;
					employee.Email = request.Email;
					employee.PhoneNumber = request.PhoneNumber;
					employee.HomeAddress = request.HomeAddress;
					employee.MaritalStatus = request.MaritalStatus;

					_dbContext.Emlpoyees.Update(employee);

					if (await _dbContext.SaveChangesAsync() > 0)
					{
						_logger.LogInformation("Saving changes");
						return new BaseResponse<Guid>
						{
							Message = "Employee Record updated successfully",
							IsSuccessful = true,
							Data = employee.Id
						};
					}
				}

				_logger.LogInformation("Failed to Update record for employee {id}", id);
				return new BaseResponse<Guid>
				{
					Message = "Record not found",
					IsSuccessful = false,
					Data = Guid.Empty
				};
			}
			catch (Exception ex)
			{
				_logger.LogError("An error occured");
				return new BaseResponse<Guid>
				{
					Message = $"Error :  {ex.Message}",
					IsSuccessful = false,
					Data = Guid.Empty
				};
			}
		}

		public async Task<BaseResponse<bool>> DeleteEmployeeAsync(Guid id, Guid companyId)
		{
			try
			{
				_logger.LogInformation("Delete employee records method called");
				var employee = await _employeeRepository.GetEmployeeAsync(id);

				if (employee != null)
				{
					_dbContext.Emlpoyees.Remove(employee);

					if (await _dbContext.SaveChangesAsync() > 0)
					{
						_logger.LogInformation("Employee record deleted successfully");
						return new BaseResponse<bool>
						{
							Message = "Employee deleted successfully",
							IsSuccessful = true,
							Data = true
						};
					}
				}

				_logger.LogInformation("Employee not found");
				return new BaseResponse<bool>
				{
					Message = "Employee not found",
					IsSuccessful = false,
					Data = false
				};
			}
			catch (Exception ex)
			{
				_logger.LogError("An error occured while processing request");
				return new BaseResponse<bool>
				{
					Message = $"Error :  {ex.Message}",
					IsSuccessful = false,
					Data = false
				};
			}
		}

    }
}