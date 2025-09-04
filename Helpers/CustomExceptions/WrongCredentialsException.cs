namespace alderaan_shop.Helpers.CustomExceptions;

public class WrongCredentialsException : Exception
{
    public WrongCredentialsException(string message) : base(message) {}
}