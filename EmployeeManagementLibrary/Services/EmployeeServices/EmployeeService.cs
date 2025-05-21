using EmployeeManagementLibrary.AppDbContext;
using EmployeeManagementLibrary.Dto;
using EmployeeManagementLibrary.Dto.Employees;
using EmployeeManagementLibrary.Repository.EmployeeRepo;
using EmployeeManagementMVC.Models;

namespace EmployeeManagementLibrary.Services.EmployeeServices
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
		private readonly ApplicationDbContext _dbContext;

		public EmployeeService(IEmployeeRepository employeeRepository, ApplicationDbContext dbContext)
		{
			_employeeRepository = employeeRepository;
			_dbContext = dbContext;
		}


		public async Task<BaseResponse<Guid>> AddEmployeeAsync(AddEmployeeDto request)
		{
			try
			{
				var employee = await _employeeRepository.GetEmployeeByFirstNameAsync(request.FirstName);
				if (employee != null)
				{
					return new BaseResponse<Guid>
					{
						Message = "Employee Record Already Exist",
						IsSuccessful = false,
						Data = Guid.Empty 
					};
				}

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
					return new BaseResponse<Guid>
					{
						Message = "Employee added successfully",
						IsSuccessful = true,
						Data = newEmployee.Id
					};
				}
				return new BaseResponse<Guid>
				{
					Message = "Failed To Create Employee",
					IsSuccessful = false,
					Data = Guid.Empty
				};
			}
			catch (Exception ex)
			{
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
				var employee = await _employeeRepository.GetEmployeeAsync(id);
				if (employee != null)
				{
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
					return new BaseResponse<EmployeeDto>
					{
						Message = "Record Retrieved Successfully",
						IsSuccessful = true,
						Data = data
					};
				}

				return new BaseResponse<EmployeeDto>
				{
					Message = "No Record Found",
					IsSuccessful = false,
					Data = null
				};
			}
			catch (Exception ex)
			{
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
                var employee = await _employeeRepository.GetCompanyByEmployeeIdAsync(employeeId);

                if (employee != null)
                {
                    return new BaseResponse<Guid>
                    {
                        Message = "Company ID Retrieved Successfully",
                        IsSuccessful = true,
                        Data = employee.CompanyId 
                    };
                }

                return new BaseResponse<Guid>
                {
                    Message = "No Record Found",
                    IsSuccessful = false,
                    Data = Guid.Empty 
                };
            }
            catch (Exception ex)
            {
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
                var employees = await _employeeRepository.GetEmployeesByCompanyIdAsync(companyId);

                if (employees != null && employees.Count > 0)
                {
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

                    return new BaseResponse<List<EmployeeDto>>
                    {
                        Message = "Employees retrieved successfully",
                        IsSuccessful = true,
                        Data = employeeDtos
                    };
                }

                return new BaseResponse<List<EmployeeDto>>
                {
                    Message = "No employees found for this company.",
                    IsSuccessful = false,
                    Data = new List<EmployeeDto>()
                };
            }
            catch (Exception ex)
            {
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
				var employee = await _employeeRepository.GetEmployeeAsync(id);
				if (employee != null)
				{	
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
						return new BaseResponse<Guid> 
						{ 
							Message = "Employee Record updated successfully", 
							IsSuccessful = true, 
							Data = employee.Id 
						};
					}
				}

				return new BaseResponse<Guid> 
				{ Message = "Record not found", 
					IsSuccessful = false, 
					Data = Guid.Empty 
				};
			}
			catch (Exception ex)
			{
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
				var employee = await _employeeRepository.GetEmployeeAsync(id);

				if (employee != null)
				{
					_dbContext.Emlpoyees.Remove(employee);

					if (await _dbContext.SaveChangesAsync() > 0)
					{
						return new BaseResponse<bool> 
						{ 
							Message = "Employee deleted successfully", 
							IsSuccessful = true, 
							Data = true 
						};
					}
				}

				return new BaseResponse<bool> 
				{ 
					Message = "Employee not found", 
					IsSuccessful = false, 
					Data = false 
				};
			}
			catch (Exception ex)
			{

				return new BaseResponse<bool> 
				{ Message = $"Error :  {ex.Message}", 
					IsSuccessful = false, 
					Data = false 
				};
			}
		}

    }
}