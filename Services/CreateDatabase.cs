using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MotorMemo.Models.Context;


namespace MotorMemo.Services
{
    public class CreateDatabase
    {
        private readonly IOptions<UserSettings> Settings;

        public CreateDatabase(IOptions<UserSettings> _settings)
        {
            Settings = _settings;
        }

        public void newDb(int firm_code, string div_id)
        {
            string dbFile = Settings.Value.dbPath + "\\" + firm_code.ToString() + div_id + ".db";

            if (!Directory.Exists(Settings.Value.dbPath))
            {
                Directory.CreateDirectory(Settings.Value.dbPath);
            }

            // Use FileStream to create the file
            using (FileStream fs = File.Create(dbFile))
            {
                // Optionally write some initial content to the file
            }
        }

        public void CreateDbAndSampleData(IServiceProvider applicationServices)
        {
            using (var serviceScope = applicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<MotorMemoDbContext>();
                context.Database.EnsureCreated();
            }
        }
    }
}
