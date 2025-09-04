namespace alderaan_shop.Helpers.CustomExceptions;

public class ProductDoesNotExistException : Exception
{
    public ProductDoesNotExistException(string message) : base(message) {}
}