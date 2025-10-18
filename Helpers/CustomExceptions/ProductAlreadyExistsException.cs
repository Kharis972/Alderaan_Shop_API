namespace alderaan_shop.Helpers.CustomExceptions;

public class ProductAlreadyExistsException : Exception
{
    public ProductAlreadyExistsException(string message) : base(message) {}
}