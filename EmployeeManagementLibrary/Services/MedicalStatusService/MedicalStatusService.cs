using System.Data.Common;
using EmployeeManagementLibrary.AppDbContext;
using EmployeeManagementLibrary.Dto;
using EmployeeManagementLibrary.Dto.MedicalStatus;
using EmployeeManagementMVC.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmployeeManagementLibrary.Services.MedicalStatusService
{
    public class MedicalStatusService : IMedicalStatusService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MedicalStatusService> _logger;

        public  MedicalStatusService(ApplicationDbContext context, ILogger<MedicalStatusService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<BaseResponse<Guid>> AddMedicalStatusAsync(AddMedicalStatusDto request)
        {
            if (request == null)
            {
                _logger.LogWarning("Add medical status request was null");
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
                bool recordExists = await _context.MedicalStatuses
                    .AnyAsync(x => x.employeeId == request.employeeId);
                
                if (recordExists)
                {
                    _logger.LogWarning("Employee {EmployeeId} already has a medical status record", request.employeeId);
                    return new BaseResponse<Guid>
                    {
                        IsSuccessful = false,
                        Message = "Employee already has a medical status record",
                        Data = Guid.Empty
                    };
                }

                var newRecord = new MedicalStatus
                {
                    Id = Guid.NewGuid(),
                    employeeId = request.employeeId,
                    BloodGroup = request.BloodGroup,
                    BloodType = request.BloodType,
                    Allergies = request.Allergies ?? string.Empty, // Handle null
                    Precautions = request.Precautions ?? string.Empty, // Handle null
                    Comment = request.Comment ?? string.Empty, // Handle null
                    CreatedDate = DateTime.UtcNow // If tracking
                };

                await _context.MedicalStatuses.AddAsync(newRecord);
                int saved = await _context.SaveChangesAsync();

                if (saved > 0)
                {
                    _logger.LogInformation("Medical status record added for employee {EmployeeId}, new record ID: {RecordId}", 
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
                _logger.LogError(ex, "Unexpected error adding medical status for employee {EmployeeId}", request.employeeId);
                return new BaseResponse<Guid>
                {
                    IsSuccessful = false,
                    Message = "An unexpected error occurred", // Generic in production
                    Data = Guid.Empty
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteMedicalStatusAsync(Guid id)
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
                var record = await _context.MedicalStatuses.FindAsync(id);
                if (record == null)
                {
                    _logger.LogWarning("Medical status record with ID {Id} not found", id);
                    return new BaseResponse<bool>
                    {
                        IsSuccessful = false,
                        Message = $"Medical status record with ID {id} not found",
                        Data = false
                    };
                }

                _context.MedicalStatuses.Remove(record);
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
                _logger.LogError(ex, "Database error while deleting medical status for ID: {Id}", id);
                return new BaseResponse<bool>
                {
                    IsSuccessful = false,
                    Message = "Database operation failed", 
                    Data = false
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error deleting medical status for ID: {Id}", id);
                return new BaseResponse<bool>
                {
                    IsSuccessful = false,
                    Message = "An unexpected error occurred",
                    Data = false
                };
            }
        }

        public async Task<BaseResponse<MedicalStatusDto>> GetMedicalStatusAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Empty GUID provided for medical status lookup");
                return new BaseResponse<MedicalStatusDto>
                {
                    IsSuccessful = false,
                    Message = "Invalid ID provided",
                    Data = null
                };
            }

            try
            {
                var record = await _context.MedicalStatuses.FindAsync(id);
                
                if (record == null)
                {
                    _logger.LogInformation("No medical status record found for ID: {Id}", id);
                    return new BaseResponse<MedicalStatusDto>
                    {
                        IsSuccessful = false,
                        Message = "Medical status record not found",
                        Data = null
                    };
                }

                _logger.LogDebug("Successfully retrieved medical status for ID: {Id}", id);
                
                return new BaseResponse<MedicalStatusDto>
                {
                    IsSuccessful = true,
                    Message = "Record retrieved successfully",
                    Data = new MedicalStatusDto
                    {
                        Id = record.Id,
                        employeeId = record.employeeId, // If needed
                        BloodGroup = record.BloodGroup,
                        BloodType = record.BloodType,
                        Allergies = record.Allergies,
                        Precautions = record.Precautions,
                        Comment = record.Comment,
                        // Add any additional fields as needed
                    }
                };
            }
            catch (DbException ex)
            {
                _logger.LogError(ex, "Database error while fetching medical status for ID: {Id}", id);
                return new BaseResponse<MedicalStatusDto>
                {
                    IsSuccessful = false,
                    Message = "Database error occurred",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error fetching medical status for ID: {Id}", id);
                return new BaseResponse<MedicalStatusDto>
                {
                    IsSuccessful = false,
                    Message = "An unexpected error occurred",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<Guid>> UpdateMedicalStatusAsync(Guid id, UpdateMedicalStatusDto request)
        {
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
                var record = await _context.MedicalStatuses
                    .FindAsync(id);

                if (record == null)
                {
                    _logger.LogWarning("Medical status record not found for ID: {Id}", id);
                    return new BaseResponse<Guid>
                    {
                        IsSuccessful = false,
                        Message = "Medical status record not found",
                        Data = Guid.Empty
                    };
                }

                record.BloodGroup = request.BloodGroup ?? record.BloodGroup;
                record.BloodType = request.BloodType ?? record.BloodType;
                record.Allergies = request.Allergies ?? record.Allergies;
                record.Precautions = request.Precautions ?? record.Precautions;
                record.Comment = request.Comment ?? record.Comment;
                record.ModifiedDate = DateTime.UtcNow;

                int saved = await _context.SaveChangesAsync();

                if (saved >= 1)
                {
                    _logger.LogInformation("Medical status updated for ID: {Id}", id);
                    return new BaseResponse<Guid>
                    {
                        IsSuccessful = true,
                        Message = "Medical status updated successfully",
                        Data = record.Id
                    };
                }

                _logger.LogWarning("No changes saved for medical status ID: {Id}", id);
                return new BaseResponse<Guid>
                {
                    IsSuccessful = false,
                    Message = "No changes were made",
                    Data = Guid.Empty
                };
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error updating medical status ID: {Id}", id);
                return new BaseResponse<Guid>
                {
                    IsSuccessful = false,
                    Message = "Failed to update medical status due to database error",
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
                _logger.LogError(ex, "Unexpected error updating medical status ID: {Id}", id);
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