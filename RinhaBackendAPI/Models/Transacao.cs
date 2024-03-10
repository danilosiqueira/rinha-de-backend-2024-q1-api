namespace RinhaBackendAPI.Models
{
    public class Transacao
    {
        public int Id { get; set; }
        public Int64 Valor { get; set; }
        public string Tipo { get; set; }
        public string Descricao { get; set; }
        public int ClienteId { get; set; }
    }
}