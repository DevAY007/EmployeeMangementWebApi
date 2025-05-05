using EmployeeManagementLibrary.Dto;
using EmployeeManagementLibrary.Dto.Company;

namespace EmployeeManagementLibrary.Services.CompanyService
{
    public interface ICompanyService
    {
      Task<BaseResponse<Guid>> CreateCompany(CreateCompanyDto request);
		Task<BaseResponse<List<CompanyRegistrationDto>>> GetAllCompany();
		Task<BaseResponse<CompanyRegistrationDto>> GetCompany(Guid id);
		Task<BaseResponse<bool>> UpdateCompany(Guid id, UpdateCompanyDto request);
		Task<BaseResponse<bool>> Delete(Guid id);
		//Task <BaseResponse<CompanyRegistrationDto>>BackToCompany();
    }
}