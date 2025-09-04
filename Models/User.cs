namespace alderaan_shop.Models;

public class User
{
    //properties:
    
    public Guid Id { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Address { get; init; }
    public string ZipCode { get; init; }
    public string Mail { get; init; }
    public string PhoneNumber { get; init; }
    public string EncryptedPassword { get; init; }
    public bool IsAdmin { get; init; }
    
    //retrouve les données hashées du client grace au hmac dans la DB 
    // Index de recherche en base de données
    // Utilise un algorithme de hachage déterministe
    // AES-GCM n'étant pas déterministe, il est impossible de comparer des données reçues à des données en DB
    // Ne pose pas de risque de sécurité en cas de fuite de données, ca il est impossible de "déhacher"
    public string MailUniqueHMac { get; init; }
    public string PhoneNumberUniqueHMac { get; init; }
    public int TriesBeforeLockedOut { get; set; }
    public bool IsActive { get; init; }
    
    //ID unique du JWT (pour empécher de se connecter à un compte existant)
    public Guid JTI { get; set; } 
    public DateTime SessionExpiresAt { get; set; }
    public string SecurityQuestion { get; init; }
    public string SecurityAnswer { get; init; }
    
    //public IEnumerable<Order>? Orders { get; set; }
    //public IEnumerable<Notification>? Notifications { get; set; }
    //public IEnumerable<ArticleInCart>? ArticlesInCart { get; set; }
    
    private User() {}

    public User
    (
        Guid id,
        Memory<char> firstName,
        Memory<char> lastName,
        Memory<char> address,
        Memory<char> zipCode,
        Memory<char> mail,
        Memory<char> phoneNumber,
        Memory<char> encryptedPassword,
        Memory<char> mailUniqueHMac,
        Memory<char> phoneNumberUniqueHMac,
        Memory<char> securityQuestion,
        Memory<char> securityAnswer,
        Memory<char> jti,
        DateTime sessionExpiresAt
    )
    {
        Id = id;
        FirstName = firstName.ToString();
        LastName = lastName.ToString();
        Address = address.ToString();
        ZipCode = zipCode.ToString();
        Mail = mail.ToString();
        PhoneNumber = phoneNumber.ToString();
        EncryptedPassword = encryptedPassword.ToString();
        IsAdmin = false;
        MailUniqueHMac = mailUniqueHMac.ToString();
        PhoneNumberUniqueHMac = phoneNumberUniqueHMac.ToString();
        TriesBeforeLockedOut = 5;
        IsActive = false;
        JTI = Guid.Parse(jti.ToString());
        SessionExpiresAt = sessionExpiresAt;
        SecurityQuestion = securityQuestion.ToString();
        SecurityAnswer = securityAnswer.ToString();
    } 
}