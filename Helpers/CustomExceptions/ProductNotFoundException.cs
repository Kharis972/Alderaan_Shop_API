namespace alderaan_shop.Helpers.CustomExceptions;

public class ProductNotFoundException : Exception
{
    public ProductNotFoundException(string messsage) : base(messsage) {}
}