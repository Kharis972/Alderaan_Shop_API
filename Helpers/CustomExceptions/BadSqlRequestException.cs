namespace alderaan_shop.Helpers.CustomExceptions;

public class BadSqlRequestException : Exception
{
    public BadSqlRequestException(string message): base(message) {}
}