namespace alderaan_shop.Helpers.CustomExceptions;

public class IncorretLoginInfosException : Exception
{
    public IncorretLoginInfosException(string message): base(message) {}
}