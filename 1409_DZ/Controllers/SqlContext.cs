using Microsoft.Data.SqlClient;

namespace _1409_DZ.Controllers
{
    public class SqlContext
    {
        private readonly string connectionString;
        public SqlContext(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:DefaultConnection"];
        }
        public void ExecuteQuery(string query, Action<SqlDataReader> action)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    action(reader);
                }
            }
        }
    }
}