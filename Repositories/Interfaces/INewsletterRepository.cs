using alderaan_shop.Models;

namespace alderaan_shop.Repositories.Interfaces;

/// <summary>
/// Contrat du repository pour l'entité Newsletter.
/// </summary>
public interface INewsletterRepository : IRepository<Newsletter>
{
    Task<Newsletter?> FindByEmailAsync(string email);
    Task<IEnumerable<Newsletter>> GetSubscribedAsync();
}
