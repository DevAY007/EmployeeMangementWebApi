using EmployeeManagementLibrary.Dto;
using EmployeeManagementLibrary.Dto.Employees;

namespace EmployeeManagementLibrary.Services.EmployeeServices
{
    public interface IEmployeeService
    {
        Task<BaseResponse<Guid>> AddEmployee(AddEmployeeDto request);
        //Task<BaseResponse<List<EmployeeDto>>> GetAllEmployee();
        Task<BaseResponse<EmployeeDto>> GetEmployee(Guid id);
        Task<BaseResponse<Guid>> UpdateEmployee(Guid id, UpdateEmployeeDto request, Guid companyId);
        Task<BaseResponse<bool>> Delete(Guid id, Guid companyId);
        Task<BaseResponse<List<EmployeeDto>>> GetAllEmployeesByCompanyId(Guid companyId);
        Task<BaseResponse<Guid>> GetCompanyIdByEmployeeId(Guid employeeId);
    }
}