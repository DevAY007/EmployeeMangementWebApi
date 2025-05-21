using EmployeeManagementLibrary.Dto;
using EmployeeManagementLibrary.Dto.Company;

namespace EmployeeManagementLibrary.Services.CompanyService
{
    public interface ICompanyService
    {
      Task<BaseResponse<Guid>> CreateCompanyAsync(CreateCompanyDto request);
		Task<BaseResponse<List<CompanyRegistrationDto>>> GetAllCompanyAsync();
		Task<BaseResponse<CompanyRegistrationDto>> GetCompanyAsync(Guid id);
		Task<BaseResponse<bool>> UpdateCompanyAsync(Guid id, UpdateCompanyDto request);
		Task<BaseResponse<bool>> DeleteCompanyAsync(Guid id);
		//Task <BaseResponse<CompanyRegistrationDto>>BackToCompany();
    }
}