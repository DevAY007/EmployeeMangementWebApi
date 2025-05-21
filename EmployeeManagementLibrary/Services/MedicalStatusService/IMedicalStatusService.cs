using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagementLibrary.Dto;
using EmployeeManagementLibrary.Dto.MedicalStatus;

namespace EmployeeManagementLibrary.Services.MedicalStatusService
{
    public interface IMedicalStatusService
    {
       Task<BaseResponse<Guid>> AddMedicalStatusAsync(AddMedicalStatusDto request);
        Task<BaseResponse<MedicalStatusDto>> GetMedicalStatusAsync(Guid id);
        Task<BaseResponse<Guid>> UpdateMedicalStatusAsync(Guid id, UpdateMedicalStatusDto request);
        Task<BaseResponse<bool>> DeleteMedicalStatusAsync(Guid id);
    }
}