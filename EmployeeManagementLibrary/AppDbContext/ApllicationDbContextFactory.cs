using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EmployeeManagementLibrary.AppDbContext
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // Assuming the appsettings.json file is in the API project.
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Ensure the path is correct for your solution's directory.
                .AddJsonFile("appsettings.json") // This will look for appsettings.json in the current directory (API project directory).
                .Build();

            // Get connection string from appsettings.json
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString); // You can replace UseSqlServer with your DB provider if using something else (e.g., SQLite).

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
