using alderaan_shop.Data;
using alderaan_shop.Models;
using alderaan_shop.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace alderaan_shop.Repositories;

public class ProductRepository(ApplicationDatabaseContext dbContext, ILogger<Repository<Product>> logger)
: Repository<Product>(dbContext, logger), IProductRepository
{
    public async Task<Product> GetProductByIdAsync(Guid id)
    {
        try
        {
            return await DbSet.FindAsync(id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        try
        {
            return await DbSet.ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<Product> AddProductAsync(Product product)
    {
        try
        {
            await DbSet.AddAsync(product);
            await DbContext.SaveChangesAsync();
            return product;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
}