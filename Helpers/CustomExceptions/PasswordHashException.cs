namespace alderaan_shop.Helpers.CustomExceptions;

public class PasswordHashException : Exception
{
    public PasswordHashException(string message) : base(message) {}
}