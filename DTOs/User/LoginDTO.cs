using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace alderaan_shop.DTOs.User;

public class LoginDTO
{
    [Required(ErrorMessage = "L'adresse e-mail est obligatoire.")]
    [StringLength(150, MinimumLength = 10, ErrorMessage = "L'adresse email doit être comprise entre 10 et 150 caractères.")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Le format n'est pas correct.")]
    [EmailAddress(ErrorMessage = "L'adresse email doit être formatter correctement.")]
    public required string Mail { get; set; }

    [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
    [StringLength(100, MinimumLength = 12, ErrorMessage = "Le mot de passe doit être compris entre 12 et 100 caractères.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&(),.?:{}|<>])[A-Za-z\d!@#$%^&(),.?:{}|<>]{12,100}$", 
        ErrorMessage = "Mot de passe trop faible. Règles non respectées : 12+ caractères, dont une majuscule, une minuscule, un chiffre et un caractère spécial.")]
    public required string Password { get; set; }
    
    [NotMapped]
    public Memory<char> MailMemoryChar;
    [NotMapped]
    public Memory<char> PasswordMemoryChar;
    
    public void InitializeMemoryChar() 
    {
        MailMemoryChar = Mail.ToCharArray().AsMemory();
        PasswordMemoryChar = Password.ToCharArray().AsMemory();
    }
}