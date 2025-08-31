using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;


namespace Data
{
    public class MyContextFactory : IDesignTimeDbContextFactory<MyContext>
    {
        public MyContext CreateDbContext(string[] args)
        {
            var currentDir = Directory.GetCurrentDirectory();
            var config = new ConfigurationBuilder()
                .SetBasePath(currentDir)
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<MyContext>();
            optionsBuilder.UseSqlServer(config.GetConnectionString("Connect"));

            return new MyContext(optionsBuilder.Options);
        }
    }
}
