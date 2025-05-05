using AspNetCoreHero.ToastNotification.Abstractions;
using EmployeeManagementLibrary.AppDbContext;
using EmployeeManagementLibrary.Dto;
using EmployeeManagementLibrary.Dto.User;
using EmployeeManagementMVC.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmployeeManagementLibrary.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(ApplicationDbContext context, UserManager<User> userManager,
            SignInManager<User> signInManager, IHttpContextAccessor httpContextAccessor, 
            RoleManager<IdentityRole> roleManager, ILogger<UserService> logger)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _roleManager = roleManager;
            _logger = logger;
        }

    public async Task<BaseResponse<UserDto>> GetUserByIdAsync(Guid id)
{
    if (id == Guid.Empty)
    {
        return new BaseResponse<UserDto>
        {
            IsSuccessful = false,
            Message = "Invalid user identifier"
        };
    }

    try
    {
        var user = await _context.Users
            .Where(x => x.Id == id.ToString())
            .Select(x => new UserDto
            {
                Id = Guid.Parse(x.Id),
                Name = x.Name,
                UserName = x.UserName,
                Email = x.Email,
            })
            .FirstOrDefaultAsync();

        if (user == null)
        {
            return new BaseResponse<UserDto>
            {
                IsSuccessful = false,
                Message = "User not found"
            };
        }

        return new BaseResponse<UserDto>
        {
            IsSuccessful = true,
            Message = "User retrieved successfully",
            Data = user
        };
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error retrieving user with ID {UserId}", id);
        
        return new BaseResponse<UserDto>
        {
            IsSuccessful = false,
            Message = "An error occurred while retrieving the user"
        };
    }
}


        public async Task<BaseResponse<bool>> UpdateUserAsync(Guid id, UpdateUserDto request)
        {
            try
            {
                var userExist = await _context.Users.FirstOrDefaultAsync(x => x.Id == id.ToString());

                if (userExist == null)
                    return new BaseResponse<bool>() { IsSuccessful = false, Message = "No record found", Data = false };

                userExist.UserName = request.UserName;
                userExist.Name = request.Name;
                userExist.Email = request.Email;

                _context.Users.Update(userExist);

                if (await _context.SaveChangesAsync() > 0)
                {
                    return new BaseResponse<bool>()
                    {
                        IsSuccessful = true,
                        Message = "Data updated successfully",
                        Data = true
                    };
                }

                return new BaseResponse<bool>()
                {
                    IsSuccessful = false,
                    Message = "Update failed",
                    Data = false
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    IsSuccessful = false,
                    Message = "UserService : UpdateUserAsync : Error Occurred:"
                };
            }
        }


        public async Task<BaseResponse<bool>> DeleteUserAsync(Guid id)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id.ToString());

                if (user == null)
                    return new BaseResponse<bool>() { IsSuccessful = false, Message = "No record found", Data = false };

                _context.Users.Remove(user);

                if (await _context.SaveChangesAsync() > 0)
                {
                    return new BaseResponse<bool>()
                    {
                        IsSuccessful = true,
                        Message = "Data deleted successfully",
                        Data = true
                    };
                }

                return new BaseResponse<bool>()
                {
                    IsSuccessful = false,
                    Message = "Delete failed",
                    Data = false
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    IsSuccessful = false,
                    Message = "UserService : DeleteUserAsync : Error Occurred:"
                };
            }
        }


        public async Task<BaseResponse<bool>> UserLogin(UserLoginRequestDto request)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(request.UserName);

                var result = await _signInManager
                    .PasswordSignInAsync(user!.UserName!, request.Password, false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return new BaseResponse<bool> { IsSuccessful = true, Message = "You're Logged in successfully" };
                }
                return new BaseResponse<bool> { IsSuccessful = true, Message = "Invald Login Attempt" };
            }
             catch (Exception ex)
            {
                return new BaseResponse<bool> { IsSuccessful = false, Message = "Invald Login Attempt" };
            }
        }

       

        public async Task<BaseResponse<bool>> UserRegistration(AddUserDto request)
        {
            try
            {
                // Check if the user already exists
                var existingUser = await _userManager.Users
                    .SingleOrDefaultAsync(u => u.UserName == request.UserName || u.Email == request.Email);

                if (existingUser != null)
                {
                    string errorMessage = existingUser.UserName == request.UserName
                        ? "Username already exists!"
                        : "Email already exists!";

                    return new BaseResponse<bool> { IsSuccessful = false, Message = errorMessage };
                }

                // Create a new user
                var user = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = request.UserName,
                    Email = request.Email,
                    Name = request.Name,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                {
                    return new BaseResponse<bool>
                    {
                        IsSuccessful = false,
                        Message = "User creation failed: " + string.Join(", ", result.Errors.Select(e => e.Description))
                    };
                }

                // Ensure role exists
                const string defaultRole = "User";
                if (!await _roleManager.RoleExistsAsync(defaultRole))
                {
                    await _roleManager.CreateAsync(new IdentityRole(defaultRole));
                }

                // Assign the user to the default role
                var roleResult = await _userManager.AddToRoleAsync(user, defaultRole);
                if (!roleResult.Succeeded)
                {
                    return new BaseResponse<bool>
                    {
                        IsSuccessful = false,
                        Message = "Failed to assign user to role: " + string.Join(", ", roleResult.Errors.Select(e => e.Description))
                    };
                }

                return new BaseResponse<bool> { IsSuccessful = true, Message = "Registration successful.", Data = true };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool> { IsSuccessful = false, Message = "An error occurred: " + ex.Message };
            }
        }



        public async Task<BaseResponse<bool>> SignOutAsync()
        {
            await _signInManager.SignOutAsync();
            return new BaseResponse<bool> { IsSuccessful = true, Message = "Sign out successfully." };
        }

        public User? GetUserName(string name)
        {
            var user = _context.Users.Where(x => x.UserName == name).FirstOrDefault();
             
            if (user != null)
            {
                return user;
            }

            return null;
        }
    }
}