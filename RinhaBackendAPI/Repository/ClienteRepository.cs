using System.Data;
using Dapper;
using RinhaBackendAPI.Models;

namespace RinhaBackendAPI.Repository
{
    public class ClienteRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;
        private readonly IDbConnection conn;

        public ClienteRepository(DbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
            conn = _dbConnectionFactory.GetConnection();
        }

        public Cliente Obter(int id, bool paraAtualizar = false)
        {
            var sql = "select * from clientes where id = @Id";

            if (paraAtualizar)
                sql += " for update";

            return conn.QuerySingle<Cliente>(sql, new { Id = id });
        }

        public void AtualizarSaldo(int id, Int64 saldo)
        {
            var sql = "update clientes set saldo = @Saldo where id = @Id";
            conn.Execute(sql, new { Saldo = saldo, Id = id });
        }
    }
}