using Microsoft.EntityFrameworkCore;
using EmployeeManagementLibrary.AppDbContext;
using EmployeeManagementLibrary.Services.CompanyService;
using EmployeeManagementLibrary.Repository.Company;
using EmployeeManagementLibrary.Repository;
using EmployeeManagementLibrary.Services;
using EmployeeManagementMVC.Models.Entities;
using Microsoft.AspNetCore.Identity;
using EmployeeManagementLibrary.Services.EmployeeServices;
using EmployeeManagementLibrary.Repository.EmployeeRepo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();



// Register your services
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

//Register you Repositories
builder.Services.AddScoped<ICompanyRepo, CompanyRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();


// Optional: Configure CORS if needed
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.MapControllers(); // Enable attribute routing via controllers

app.Run();
