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
using Serilog;
using EmployeeManagementLibrary.Services.MedicalStatusService;
using EmployeeManagementLibrary.Services.EducationHistoryServices;
using EmployeeManagementLibrary.Services.WorkHistoryService;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using EmployeeManagementMVC.Models.JwtSettings;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Diagnostics;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    Log.Information("starting server.");
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, loggerConfiguration) =>
    {
        loggerConfiguration.WriteTo.Console();
        loggerConfiguration.ReadFrom.Configuration(context.Configuration);
    });

    // Add services to the container
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        var jwtSecurityScheme = new OpenApiSecurityScheme
        {
            BearerFormat = "JWT",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            Description = "Enter your Access Token",
            Reference = new OpenApiReference
            {
                Id = JwtBearerDefaults.AuthenticationScheme,
                Type = ReferenceType.SecurityScheme
            }
        };

        options.AddSecurityDefinition("Bearer", jwtSecurityScheme);
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            { jwtSecurityScheme, Array.Empty<string>() }
        });
    });

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddIdentity<User, IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

    // Register services
    builder.Services.AddScoped<ICompanyService, CompanyService>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IEmployeeService, EmployeeService>();
    builder.Services.AddScoped<IWorkHistoryService, WorkHistoryService>();
    builder.Services.AddScoped<IMedicalStatusService, MedicalStatusService>();
    builder.Services.AddScoped<IEducationHistoryService, EducationHistoryService>();
    builder.Services.AddScoped<JwtService>();

    // Register repositories
    builder.Services.AddScoped<ICompanyRepo, CompanyRepository>();
    builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

    // CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
    });

    

    //builder.Services.Configure<LoginRequestModel>(builder.Configuration.GetSection("JwtSettings"));
    //var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<LoginResponseModel>();
    //var key = Encoding.ASCII.GetBytes(jwtSettings.SecretKey);

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            //ValidIssuer = jwtSettings.Issuer,
            //ValidAudience = jwtSettings.Audience,
            ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
            ValidAudience = builder.Configuration["JwtConfig:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Key"])),
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true
        };
    });
    builder.Services.AddAuthorization();

    var app = builder.Build();

    // Global error handler
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/plain";

            var error = context.Features.Get<IExceptionHandlerFeature>();
            if (error != null)
            {
                await context.Response.WriteAsync(error.Error.Message);
            }
        });
    });

    // Swagger always enabled
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHttpsRedirection();
    app.UseCors("AllowAll");
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "server terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
