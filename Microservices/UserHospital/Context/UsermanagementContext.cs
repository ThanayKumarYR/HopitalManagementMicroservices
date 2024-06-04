using Microsoft.Data.SqlClient;
using System.Data;

namespace UserHospital.Context
{
    public class UsermanagementContext
    {
        private readonly IConfiguration _configuration;

        private readonly string _connection;

        public UsermanagementContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = _configuration.GetConnectionString("UserManagementConnection");
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connection);

    }
}
