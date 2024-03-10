using System.Data;
using Dapper;
using RinhaBackendAPI.Models;

namespace RinhaBackendAPI.Repository
{
    public class TransacaoRepository
    {
        private readonly DbConnectionFactory _dbConnectionFactory;
        private readonly IDbConnection conn;

        public TransacaoRepository(DbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
            conn = _dbConnectionFactory.GetConnection();
        }

        public Transacao Criar(Transacao transacao)
        {
            var sql = @"insert into transacoes (valor, tipo, descricao, cliente_id)
            values (@Valor, @Tipo, @Descricao, @ClienteId) returning id";
            transacao.Id = conn.ExecuteScalar<int>(sql, transacao);
            return transacao;
        }
    }
}