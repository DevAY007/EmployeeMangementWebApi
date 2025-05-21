using EmployeeManagementLibrary.Dto;
using EmployeeManagementLibrary.Dto.Employees;

namespace EmployeeManagementLibrary.Services.EmployeeServices
{
    public interface IEmployeeService
    {
        Task<BaseResponse<Guid>> AddEmployeeAsync(AddEmployeeDto request);
        Task<BaseResponse<EmployeeDto>> GetEmployeeAsync(Guid id);
        Task<BaseResponse<Guid>> UpdateEmployeeAsync(Guid id, UpdateEmployeeDto request, Guid companyId);
        Task<BaseResponse<bool>> DeleteEmployeeAsync(Guid id, Guid companyId);
        Task<BaseResponse<List<EmployeeDto>>> GetAllEmployeesByCompanyIdAsync(Guid companyId);
        Task<BaseResponse<Guid>> GetCompanyIdByEmployeeIdAsync(Guid employeeId);
    }
}