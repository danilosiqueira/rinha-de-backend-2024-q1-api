namespace RinhaBackendAPI.Business
{
    public class InsufficientFundsException: ApplicationException
    {
        public InsufficientFundsException(string message): base(message)
        {
            
        }
    }
}