using System.Data.SqlClient;
using System.Data;

namespace PatientHistoryService.Context
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;

        private readonly string _connection;

        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = _configuration.GetConnectionString("PatientHistoryConnection");
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connection);
    }
}
