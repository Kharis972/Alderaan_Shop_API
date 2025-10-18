namespace alderaan_shop.Helpers.CustomExceptions;

public class InternalServerException : Exception
{
    public InternalServerException(string message): base(message) {}
    public InternalServerException(string message, Exception innerException) 
        : base(message, innerException) { }
}