namespace RinhaBackendAPI.Business
{
    public class CustomerNotFoundException: ApplicationException
    {
        public CustomerNotFoundException(string message): base(message)
        {
            
        }
    }
}