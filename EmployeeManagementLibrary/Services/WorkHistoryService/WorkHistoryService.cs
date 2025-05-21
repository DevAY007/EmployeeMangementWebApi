using System.Data.Common;
using System.Runtime.CompilerServices;
using EmployeeManagementLibrary.AppDbContext;
using EmployeeManagementLibrary.Dto;
using EmployeeManagementLibrary.Dto.WorkHistory;
using EmployeeManagementMVC.Models;
using EmployeeManagementMVC.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmployeeManagementLibrary.Services.WorkHistoryService
{
    public class WorkHistoryService : IWorkHistoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<WorkHistoryService> _logger;

        public WorkHistoryService(ApplicationDbContext context, ILogger<WorkHistoryService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<BaseResponse<Guid>> AddWorkHistoryAsync(AddWorkHistoryDto request)
        {
            try 
            {
                var existingRecord = await _context.WorkHistories.SingleOrDefaultAsync(x => x.employeeId == request.employeeId);

                _logger.LogWarning("Work history already exists");
                if(existingRecord != null)
                {
                    return new BaseResponse<Guid>
                    {
                        IsSuccessful = false,
                        Message = "Record Already Exists",
                        Data = Guid.Empty
                    };
                }

                var newRecord = new WorkHistory
                {
                    Id = Guid.NewGuid(),
                    employeeId = request.employeeId,
                    CompanyName = request.CompanyName,
                    Responsibility = request.Responsibility,
                    StartDate = DateTimeOffset.Now,
                    EndDate = request.EndDate,
                    IsCurrentJob = request.IsCurrentJob
                };

                await _context.WorkHistories.AddAsync(newRecord);

                if (await _context.SaveChangesAsync() > 0)
                {
                    _logger.LogInformation("Work History record added for employee {EmployeeId}, new record ID: {RecordId}", 
                        request.employeeId, newRecord.Id);
                    return new BaseResponse<Guid>
                    {
                        IsSuccessful = true,
                        Message = "Record Added Successfully",
                        Data = newRecord.Id
                    };
                }

                _logger.LogError("No changes saved when adding work history for employee {EmployeeId}", request.employeeId);
                return new BaseResponse<Guid>
                {
                    IsSuccessful = false,
                    Message = "An Error Occured",
                    Data = Guid.Empty
                };
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while adding work history for employee {EmployeeId}", request.employeeId);
                return new BaseResponse<Guid>
                {
                    IsSuccessful = false,
                    Message = "Database operation failed",
                    Data = Guid.Empty
                };
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Unexpected error adding work history for employee {EmployeeId}", request.employeeId);
                return new BaseResponse<Guid>
                {
                    IsSuccessful = false,
                    Message = $"An Error Occured {ex.Message}",
                    Data = Guid.Empty
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteWorkHistoryAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Attempted to delete with empty GUID");
                return new BaseResponse<bool>
                {
                    IsSuccessful = false,
                    Message = "Invalid ID provided",
                    Data = false
                };
            }
            try
            {
                var record = await _context.WorkHistories.FindAsync(id);
                
                if (record != null)
                {
                    _context.WorkHistories.Remove(record);

                    int saveChanges = await _context.SaveChangesAsync();

                    if (saveChanges >= 1)
                    {
                         _logger.LogInformation("Record with ID {Id} deleted successfully", id);
                        return new BaseResponse<bool> 
                        {
                            IsSuccessful = true,
                            Message = "Record Deleted Successfully",
                            Data = true
                        };
                    }
                    else
                    {
                        _logger.LogError("No changes were saved when deleting record with ID {Id}", id);
                        return new BaseResponse<bool>
                        {
                            IsSuccessful = true,
                            Message = "Failed To Delete Record (no changes saved)",
                            Data = false
                        };
                    }
                }
                else
                {
                    _logger.LogWarning("Medical status record with ID {Id} not found", id);
                    return new BaseResponse<bool>
                    {
                        IsSuccessful = false,
                        Message = $"Medical status record with ID {id} not found",
                        Data = false
                    };
                }
            }
            catch(DbUpdateException ex)
            {
                 _logger.LogError(ex, "Database error while deleting medical status for ID: {Id}", id);
                return new BaseResponse<bool>
                {
                    IsSuccessful = false,
                    Message = $"A Database Error Occured {ex.Message}",
                    Data = false
                };
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Unexpected error deleting medical status for ID: {Id}", id);
                return new BaseResponse<bool>
                {
                    IsSuccessful = false,
                    Message =  $"An Unexpected Error Occured {ex.Message}",
                    Data = false
                };
            }
        }

        public async Task<BaseResponse<WorkHistoryDto>> GetWorkHistoryAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Empty GUID provided for medical status lookup");
                return new BaseResponse<WorkHistoryDto>
                {
                    IsSuccessful = false,
                    Message = "Invalid ID provided",
                    Data = null
                };
            }
           try
           {
                var record = await _context.WorkHistories.Where(x => x.Id == id).FirstOrDefaultAsync();

                if (record == null)
                {
                    _logger.LogInformation("No medical status record found for ID: {Id}", id);
                    return new BaseResponse<WorkHistoryDto>
                    {
                        IsSuccessful = false,
                        Message = "No Record Found",
                        Data =null
                    };
                }
                
                
                var data = new WorkHistoryDto
                {
                    Id = record.Id,
                    CompanyName = record.CompanyName,
                    Responsibility = record.Responsibility,
                    StartDate = record.StartDate,
                    EndDate = record.EndDate,
                    IsCurrentJob = record.IsCurrentJob
                };

                 _logger.LogDebug("Successfully retrieved medical status for ID: {Id}", id);
                return new BaseResponse<WorkHistoryDto>
                {
                    IsSuccessful = true,
                    Message =  "Record Retrieved Successully",
                    Data = data
                };
           }
           catch (DbException ex)
            {
                _logger.LogError(ex, "Database error while fetching medical status for ID: {Id}", id);
                return new BaseResponse<WorkHistoryDto>
                {
                    IsSuccessful = false,
                    Message = "Database error occurred",
                    Data = null
                };
            }
           catch (Exception ex)
           {
             _logger.LogError(ex, "Unexpected error fetching medical status for ID: {Id}", id);
                return new BaseResponse<WorkHistoryDto>
                {
                    IsSuccessful = false,
                    Message = $"An Error Occured {ex.Message}"
                };
           }
        }

        public async Task<BaseResponse<Guid>> UpdateWorkHistoryAsync(Guid id, UpdateWorkHistoryDto request)
        {
            if (request == null)
            {
                 _logger.LogWarning("Update attempted with null request for ID: {Id}", id);
                return new BaseResponse<Guid>
                {
                    IsSuccessful = false,
                    Message = "Can't Update an Empty Field",
                    Data = Guid.Empty
                };
            }

            try
            {
                var record = await _context.WorkHistories.FindAsync(id);

                if (record == null)
                {
                     _logger.LogWarning("Medical status record not found for ID: {Id}", id);
                    return new BaseResponse<Guid>
                    {
                        IsSuccessful = false,
                        Message = "Record Not Found",
                        Data = Guid.Empty
                    };
                }

                record.CompanyName = request.CompanyName ?? record.CompanyName;
                record.Responsibility = request.Responsibility ?? record.CompanyName;
                record.StartDate = request.StartDate;
                record.EndDate = request.EndDate;
                record.IsCurrentJob = request.IsCurrentJob;
                record.ModifiedDate = DateTime.UtcNow;

                _context.WorkHistories.Update(record);

                int saved = await _context.SaveChangesAsync();

                if (saved >= 1)
                {
                    _logger.LogInformation("Medical status updated for ID: {Id}", id);
                    return new BaseResponse<Guid>
                    {
                        IsSuccessful = true,
                        Message = "Record Updated Successfully",
                        Data = record.Id
                    };
                }

                _logger.LogWarning("No changes saved for medical status ID: {Id}", id);
                return new BaseResponse<Guid>
                {
                    IsSuccessful = false,
                    Message = "Failed to save changes",
                    Data = Guid.Empty
                };
            }
            catch(DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error updating medical status ID: {Id}", id);
                return new BaseResponse<Guid>
                {
                    IsSuccessful = false,
                    Message = $"An error occured while trying to update record {ex.Message}",
                    Data = Guid.Empty
                };
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Update operation cancelled for ID: {Id}", id);
                return new BaseResponse<Guid>
                {
                    IsSuccessful = false,
                    Message = $"An error occured {ex.Message}",
                    Data = Guid.Empty
                };
            }
        }
    }
}