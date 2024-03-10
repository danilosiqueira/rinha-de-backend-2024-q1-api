using System.Data;
using Npgsql;

namespace RinhaBackendAPI
{
    public class DbConnectionFactory : IDisposable
    {
        private readonly IConfiguration configuration;
        private readonly IDbConnection conn;

        public DbConnectionFactory(IConfiguration configuration)
        {
            this.configuration = configuration;
            conn = new NpgsqlConnection(configuration["connectionString"]);
        }

        public IDbConnection GetConnection()
        {
            return conn;
        }

        public void Dispose()
        {
            conn.Dispose();
        }

    }
}