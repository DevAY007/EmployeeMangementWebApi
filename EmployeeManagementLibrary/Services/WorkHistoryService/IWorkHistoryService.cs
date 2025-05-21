using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagementLibrary.Dto;
using EmployeeManagementLibrary.Dto.WorkHistory;

namespace EmployeeManagementLibrary.Services.WorkHistoryService
{
    public interface IWorkHistoryService
    {
        Task<BaseResponse<Guid>> AddWorkHistoryAsync(AddWorkHistoryDto request);
        Task<BaseResponse<WorkHistoryDto>> GetWorkHistoryAsync(Guid id);
        Task<BaseResponse<Guid>> UpdateWorkHistoryAsync(Guid id, UpdateWorkHistoryDto request);
        Task<BaseResponse<bool>> DeleteWorkHistoryAsync(Guid id);
    }
}