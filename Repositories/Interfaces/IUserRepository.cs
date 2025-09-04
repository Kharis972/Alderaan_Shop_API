using alderaan_shop.Models;

namespace alderaan_shop.Repositories.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetUserByMailHmacAsync(string userMail);
    Task<IEnumerable<User>> GetUsersByRoleWithPagingAsync(bool isAdmin, int index, int entityNumber);
    Task<bool> CheckForUserByUniqueIndexesAsync(string phoneNumberUniqueHmac, string mailUniqueHmac);
    Task<bool> CheckForUserRoleByIdAsync(Guid id);
    Task<bool> CheckForUserByIdAsync(Guid id);
}