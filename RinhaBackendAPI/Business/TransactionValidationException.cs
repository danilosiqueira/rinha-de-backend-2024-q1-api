namespace RinhaBackendAPI.Business
{
    public class TransactionValidationException: ApplicationException
    {
        public TransactionValidationException(string message): base(message)
        {
            
        }
    }
}