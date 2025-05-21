using System.Data.Common;
using EmployeeManagementLibrary.AppDbContext;
using EmployeeManagementLibrary.Dto;
using EmployeeManagementLibrary.Dto.EducationRecord;
using EmployeeManagementMVC.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmployeeManagementLibrary.Services.EducationHistoryServices
{
    public class EducationHistoryService : IEducationHistoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EducationHistoryService> _logger;

        public EducationHistoryService(ApplicationDbContext context, ILogger<EducationHistoryService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<BaseResponse<Guid>> AddEducationRecordAsync(AddEducationRecordDto request)
        {
            if (request == null)
            {
                _logger.LogWarning("Add education record request was null");
                return new BaseResponse<Guid>
                {
                    IsSuccessful = false,
                    Message = "Request cannot be null",
                    Data = Guid.Empty
                };
            }

            if (request.employeeId == Guid.Empty)
            {
                _logger.LogWarning("Invalid EmployeeId provided");
                return new BaseResponse<Guid>
                {
                    IsSuccessful = false,
                    Message = "Invalid Employee ID",
                    Data = Guid.Empty
                };
            }
            try
            {
               bool recordExists = await _context.EducationRecords
                    .AnyAsync(x => x.employeeId == request.employeeId);
                
                if (recordExists)
                {
                    _logger.LogWarning("Employee {EmployeeId} already has a education record", request.employeeId);
                    return new BaseResponse<Guid>
                    {
                        IsSuccessful = false,
                        Message = "Employee already has a education record",
                        Data = Guid.Empty
                    };
                }

                var newRecord = new EducationHistory
                {
                    Id = Guid.NewGuid(),
                    employeeId = request.employeeId,
                    SchoolName = request.SchoolName,
                    SchoolType = request.SchoolType,
                    CourseStudied = request.CourseStudied,
                    IsGraduated = request.IsGraduated,
                    Certificates = request.Certificates
                };

                await _context.EducationRecords.AddAsync(newRecord);
                int saved = await _context.SaveChangesAsync();

                if (saved > 0)
                {
                    _logger.LogInformation("Education record added for employee {EmployeeId}, new record ID: {RecordId}", 
                        request.employeeId, newRecord.Id);
                    return new BaseResponse<Guid>
                    {
                        IsSuccessful = true,
                        Message = "Record added successfully",
                        Data = newRecord.Id
                    };
                }
                
                _logger.LogError("No changes saved when adding medical status for employee {EmployeeId}", request.employeeId);
                return new BaseResponse<Guid>
                {
                    IsSuccessful = false,
                    Message = "Failed to save record",
                    Data = Guid.Empty
                };
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while adding medical status for employee {EmployeeId}", request.employeeId);
                return new BaseResponse<Guid>
                {
                    IsSuccessful = false,
                    Message = "Database operation failed", // Generic in production
                    Data = Guid.Empty
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error adding Education record for employee {EmployeeId}", request.employeeId);
                return new BaseResponse<Guid>
                {
                    IsSuccessful = false,
                    Message = "An unexpected error occurred", // Generic in production
                    Data = Guid.Empty
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteEducationRecordsAsync(Guid id)
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
                var record = await _context.EducationRecords.FindAsync(id);
                if (record == null)
                {
                    _logger.LogWarning("Medical status record with ID {Id} not found", id);
                    return new BaseResponse<bool>
                    {
                        IsSuccessful = false,
                        Message = $"Education record with ID {id} not found",
                        Data = false
                    };
                }

                _context.EducationRecords.Remove(record);
                int saved = await _context.SaveChangesAsync();

                if (saved > 0)
                {
                    _logger.LogInformation("Record with ID {Id} deleted successfully", id);
                    return new BaseResponse<bool>
                    {
                        IsSuccessful = true,
                        Message = "Record deleted successfully",
                        Data = true
                    };
                }

                _logger.LogError("No changes were saved when deleting record with ID {Id}", id);
                return new BaseResponse<bool>
                {
                    IsSuccessful = false,
                    Message = "Failed to delete record (no changes saved)",
                    Data = false
                };
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while deleting education record for ID: {Id}", id);
                return new BaseResponse<bool>
                {
                    IsSuccessful = false,
                    Message = "Database operation failed", 
                    Data = false
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error deleting education recordz for ID: {Id}", id);
                return new BaseResponse<bool>
                {
                    IsSuccessful = false,
                    Message = "An unexpected error occurred",
                    Data = false
                };
            }
        }

        public async Task<BaseResponse<EducationRecordDto>> GetEducationRecordAsync(Guid id)
        {
             if (id == Guid.Empty)
            {
                _logger.LogWarning("Empty GUID provided for medical status lookup");
                return new BaseResponse<EducationRecordDto>
                {
                    IsSuccessful = false,
                    Message = "Invalid ID provided",
                    Data = null
                };
            }

            try
            {
                var record = await _context.EducationRecords.Where(x => x.Id == id).FirstOrDefaultAsync();
                
                if (record == null)
                {
                    _logger.LogInformation("No medical status record found for ID: {Id}", id);
                    return new BaseResponse<EducationRecordDto>
                    {
                        IsSuccessful = false,
                        Message = "Medical status record not found",
                        Data = null
                    };
                }
                

                var data = new EducationRecordDto
                {
                    Id = record.Id,
                    SchoolName = record.SchoolName,
                    SchoolType = record.SchoolType,
                    Certificates = record.Certificates,
                    CourseStudied = record.CourseStudied,
                    IsGraduated = record.IsGraduated,
                };

                _logger.LogDebug("Successfully retrieved medical status for ID: {Id}", id);
                return new BaseResponse<EducationRecordDto>
                {
                    IsSuccessful = true,
                    Message = "Record retrieved successfully",
                    Data = data
                };
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Operation cancelled while fetching education record for ID: {Id}", id);
                return new BaseResponse<EducationRecordDto>
                {
                    IsSuccessful = false,
                    Message = "Request was cancelled",
                    Data = null
                };
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Database error while fetching education record for ID: {Id}", id);
                return new BaseResponse<EducationRecordDto>
                {
                    IsSuccessful = false,
                    Message = "Database error occurred",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error fetching education record for ID: {Id}", id);
                return new BaseResponse<EducationRecordDto>
                {
                    IsSuccessful = false,
                    Message = "An unexpected error occurred",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<Guid>> UpdateEducationRecordAsync(Guid id, UpdateEducationHistoryDto request)
               {
            // Input validation
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Update attempted with empty GUID");
                return new BaseResponse<Guid>
                {
                    IsSuccessful = false,
                    Message = "Invalid ID provided",
                    Data = Guid.Empty
                };
            }

            if (request == null)
            {
                _logger.LogWarning("Update attempted with null request for ID: {Id}", id);
                return new BaseResponse<Guid>
                {
                    IsSuccessful = false,
                    Message = "Request cannot be null",
                    Data = Guid.Empty
                };
            }

            try
            {
                var record = await _context.EducationRecords
                    .FindAsync(id);
                
                if (record == null)
                {
                    _logger.LogWarning("education record not found for ID: {Id}", id);
                    return new BaseResponse<Guid>
                    {
                        IsSuccessful = false,
                        Message = "Education record not found",
                        Data = Guid.Empty
                    };
                }

                record.SchoolName = request.SchoolName ?? record.SchoolName;
                record.SchoolName = request.SchoolType ?? record.SchoolType;
                record.CourseStudied = request.CourseStudied ?? record.CourseStudied;
                record.Certificates = request.Certificates ?? record.Certificates;
                record.IsGraduated = request.IsGraduated;
                record.ModifiedDate = DateTime.UtcNow;

                int saved = await _context.SaveChangesAsync();

                if (saved > 0)
                {
                    _logger.LogInformation("Education record updated for ID: {Id}", id);
                    return new BaseResponse<Guid>
                    {
                        IsSuccessful = true,
                        Message = "Education record updated successfully",
                        Data = record.Id
                    };
                }

                _logger.LogWarning("No changes saved for education record ID: {Id}", id);
                return new BaseResponse<Guid>
                {
                    IsSuccessful = false,
                    Message = "No changes were made",
                    Data = Guid.Empty
                };
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error updating education record ID: {Id}", id);
                return new BaseResponse<Guid>
                {
                    IsSuccessful = false,
                    Message = "Failed to update eduction record due to database error",
                    Data = Guid.Empty
                };
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Update operation cancelled for ID: {Id}", id);
                return new BaseResponse<Guid>
                {
                    IsSuccessful = false,
                    Message = "Update operation was cancelled",
                    Data = Guid.Empty
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating education record ID: {Id}", id);
                return new BaseResponse<Guid>
                {
                    IsSuccessful = false,
                    Message = "An unexpected error occurred while updating",
                    Data = Guid.Empty
                };
            }
        }
    }
}