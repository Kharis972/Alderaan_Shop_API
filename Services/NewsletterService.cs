using alderaan_shop.Models;
using alderaan_shop.Repositories.Interfaces;
using alderaan_shop.Services.Interfaces;

namespace alderaan_shop.Services;

/// <summary>
/// Service newsletter.
/// </summary>
public class NewsletterService : Service<Newsletter>, INewsletterService
{
    private readonly INewsletterRepository _newsletterRepository;

    /// <summary>
    /// Initialise le service Newsletter.
    /// </summary>
    /// <param name="newsletterRepository">Repository newsletter.</param>
    /// <param name="logger">Logger typé.</param>
    public NewsletterService(INewsletterRepository newsletterRepository, ILogger<NewsletterService> logger)
        : base(newsletterRepository, logger)
    {
        _newsletterRepository = newsletterRepository;
    }

    /// <summary>
    /// Recherche un abonnement par email exact.
    /// </summary>
    /// <param name="email">Adresse email.</param>
    /// <returns>L'entrée newsletter ou null.</returns>
    /// <exception cref="ArgumentException">Si email est vide.</exception>
    public Task<Newsletter?> FindByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email requis", nameof(email));
        return _newsletterRepository.FindByEmailAsync(email);
    }

    /// <summary>
    /// Retourne les abonnements actifs.
    /// </summary>
    public Task<IEnumerable<Newsletter>> GetSubscribedAsync()
    {
        return _newsletterRepository.GetSubscribedAsync();
    }
}