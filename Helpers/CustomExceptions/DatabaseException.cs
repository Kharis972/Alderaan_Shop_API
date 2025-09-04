namespace alderaan_shop.Helpers.CustomExceptions;

public class DatabaseException : Exception
{
    public DatabaseException(string message): base(message) {} 
}