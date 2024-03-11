namespace RinhaBackendAPI.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public Int64 Limite { get; set; }
        public Int64 Saldo { get; set; }
        public IEnumerable<Transacao> Transacoes { get; set; }
    }
}