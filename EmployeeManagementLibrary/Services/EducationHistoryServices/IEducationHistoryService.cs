using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagementLibrary.Dto;
using EmployeeManagementLibrary.Dto.EducationRecord;

namespace EmployeeManagementLibrary.Services.EducationHistoryServices
{
    public interface IEducationHistoryService
    {
        Task<BaseResponse<Guid>> AddEducationRecordAsync(AddEducationRecordDto request);
        Task<BaseResponse<EducationRecordDto>> GetEducationRecordAsync(Guid id);
        Task<BaseResponse<Guid>> UpdateEducationRecordAsync(Guid id, UpdateEducationHistoryDto request);
        Task<BaseResponse<bool>> DeleteEducationRecordsAsync(Guid id);
    }
}