using RinhaBackendAPI.Models;
using RinhaBackendAPI.Repository;

namespace RinhaBackendAPI.Business
{
    public class TransacaoBusiness
    {
        private readonly DbConnectionFactory _dbConnectionFactory;
        private readonly TransacaoRepository _transacaoRepository;
        private readonly ClienteRepository _clienteRepository;

        public TransacaoBusiness(
            DbConnectionFactory dbConnectionFactory,
            TransacaoRepository transacaoRepository,
            ClienteRepository clienteRepository)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _transacaoRepository = transacaoRepository;
            _clienteRepository = clienteRepository;
        }

        public Transacao Save(Transacao transacao)
        {
            ValidarTransacao(transacao);

            var conn = _dbConnectionFactory.GetConnection();
            conn.Open();

            using (var trans = conn.BeginTransaction())
            {
                try
                {
                    var cliente = _clienteRepository.Obter(transacao.ClienteId, true);

                    if (cliente == null)
                        throw new CustomerNotFoundException($"O cliente { transacao.ClienteId } não foi encontrado.");

                    var saldo = cliente.Saldo;
                    var valorTransacao = transacao.Tipo == "d" ? transacao.Valor * -1 : transacao.Valor;
                    saldo += valorTransacao;

                    if (cliente.Limite + saldo < 0)
                        throw new InsufficientFundsException($"O saldo do cliente { cliente.Id } é insuficiente.");

                    _transacaoRepository.Criar(transacao);
                    _clienteRepository.AtualizarSaldo(cliente.Id, saldo);
                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
            }

            return transacao;
        }

        private static void ValidarTransacao(Transacao transacao)
        {
            if (transacao.Valor <= 0)
                throw new TransactionValidationException("O valor da transação deve ser maior que zero.");

            if (transacao.Tipo == null)
                throw new TransactionValidationException("O tipo da transação deve ser informado.");

            transacao.Tipo = transacao.Tipo.ToLower();
            if (transacao.Tipo != "d" && transacao.Tipo != "c")
                throw new TransactionValidationException("O tipo da transação só pode ser débito (d) ou crédito (c).");

            if (transacao.Descricao == null || transacao.Descricao.Length < 1 || transacao.Descricao.Length > 10)
                throw new TransactionValidationException("A transação deve possuir uma descrição de 1 a 10 caracteres.");

            if (transacao.ClienteId <= 0)
                throw new TransactionValidationException("O id do cliente da transação é inválido.");
        }
    }
}