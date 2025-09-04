namespace alderaan_shop.Helpers.CustomExceptions;

public class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException(string message) : base(message) {}
}