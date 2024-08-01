using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace WebApplication_Bills.Services
{
    public class Connection
    {
        private string sqlText = string.Empty;

        public Connection()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory) // Base directory instead of GetCurrentDirectory
                .AddJsonFile("appsettings.json")
                .Build();

            sqlText = builder.GetConnectionString("DefaultConnection");
        }

        public string GetSqlText()
        {
            return sqlText;
        }
    }
}
