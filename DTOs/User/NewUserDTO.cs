#pragma warning disable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using alderaan_shop.DTOs.Interfaces;

namespace alderaan_shop.DTOs.User;

public class NewUserDTO : IMemoryCharInitializable
{
    [Required(ErrorMessage = "Le prénom est obligatoire pour créer un compte.")]
    [MinLength(2, ErrorMessage = "Le prénom doit contenir au minimum 2 caractères.")]
    [MaxLength(50, ErrorMessage = "Le prénom doit contenir au maximum 50 caractères.")]
    [RegularExpression(@"^[A-Za-zÀ-ÖØ-öø-ÿ' -]{2,50}$",
        ErrorMessage = "Le prénom ne doit contenir que des lettres, et faire entre 2 et 50 caractères.")]
    public required string FirstName { get; init; }

    [Required(ErrorMessage = "Le nom est obligatoire pour créer un compte.")]
    [MinLength(2, ErrorMessage = "Le nom doit contenir au minimum 2 caractères.")]
    [MaxLength(50, ErrorMessage = "Le nom doit contenir au maximum 50 caractères.")]
    [RegularExpression(@"^[A-Za-zÀ-ÖØ-öø-ÿ' -]{2,50}$", ErrorMessage = "Le nom ne doit contenir que des lettres, et faire entre 2 et 50 caractères.")]
    public required string LastName { get; init; }
    
    [Required(ErrorMessage = "L'adresse est obligatoire pour créer un compte.")]
    [MinLength(10, ErrorMessage = "L'adresse doit contenir au minimum 10 caractères.")]
    [MaxLength(200, ErrorMessage = "L'adresse doit contenir au maximum 200 caractères.")]
    public required string Address { get; init; }
    
    [Required(ErrorMessage = "Le code postal est obligatoire pour créer un compte.")]
    [RegularExpression(@"^\d{5}$", ErrorMessage = "Le code postal doit contenir exactement 5 chiffres.")]
    public required string ZipCode { get; init; }


    [Required(ErrorMessage = "L'adresse e-mail est obligatoire pour créer un compte.")]
    [MinLength(6, ErrorMessage = "L'adresse e-mail doit contenir au minimum 6 caractères.")]
    [MaxLength(100, ErrorMessage = "L'adresse e-mail doit contenir au maximum 100 caractères.")]
    [EmailAddress(ErrorMessage = "Format d'email invalide.")]
    public required string Mail { get; init; }

    [Required(ErrorMessage = "Le numéro de téléphone est obligatoire pour créer un compte.")]
    [StringLength(10, MinimumLength = 10, ErrorMessage = "Le numéro de téléphone doit faire exactement 10 caractères.")]
    [RegularExpression(@"^0\d{9}$", ErrorMessage = "Le numéro de téléphone doit commencer par 0 et contenir exactement 10 chiffres.")]
    public required string PhoneNumber { get; init; }
    
    [Required(ErrorMessage = "Le mot de passe est obligatoire pour créer un compte.")]
    [MinLength(12, ErrorMessage = "Le mot de passe doit contenir au minimum 12 caractères.")]
    [MaxLength(36, ErrorMessage = "Le mot de passe doit contenir au maximum 36 caractères.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{12,36}$", ErrorMessage = "Le mot de passe doit contenir au moins 12 caractères, une majuscule, une minuscule, un chiffre et un caractère spécial.")]
    [DataType(DataType.Password)]
    public required string Password { get; init; }
    
    [Required(ErrorMessage = "La question de sécurité est obligatoire.")]
    [MinLength(10, ErrorMessage = "La question de sécurité doit contenir au minimum 10 caractères.")]
    [MaxLength(200, ErrorMessage = "La question de sécurité doit contenir au maximum 100 caractères.")]
    public required string SecurityQuestion { get; init; }

    [Required(ErrorMessage = "La réponse à la question de sécurité est obligatoire.")]
    [MinLength(8, ErrorMessage = "La réponse doit contenir au minimum 8 caractères.")]
    [MaxLength(100, ErrorMessage = "La réponse doit contenir au maximum 100 caractères.")]
    [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d)?[a-zA-Z\d\s\-\.,'!?]{8,100}$", ErrorMessage = "La réponse doit contenir au moins une lettre. Les chiffres sont facultatifs. Caractères spéciaux autorisés : - . , ' ! ?")]
    [DataType(DataType.Text)]
    public required string SecurityAnswer { get; init; }

    
    [NotMapped]
    public Memory<char> FirstNameMemoryChar;
    [NotMapped]
    public Memory<char> LastNameMemoryChar;
    [NotMapped]
    public Memory<char> AddressMemoryChar;
    [NotMapped]
    public Memory<char> ZipCodeMemoryChar;
    [NotMapped]
    public Memory<char> MailMemoryChar;
    [NotMapped]
    public Memory<char> PhoneNumberMemoryChar;
    [NotMapped]
    public Memory<char> PasswordMemoryChar;
    [NotMapped]
    public Memory<char> SecurityQuestionMemoryChar;
    [NotMapped]
    public Memory<char> SecurityAnswerMemoryChar;
    
    public void InitializeMemoryChar() 
    {
        FirstNameMemoryChar = FirstName.ToCharArray().AsMemory();
        LastNameMemoryChar = LastName.ToCharArray().AsMemory();
        AddressMemoryChar = Address.ToCharArray().AsMemory();
        ZipCodeMemoryChar = ZipCode.ToCharArray().AsMemory();
        MailMemoryChar = Mail.ToCharArray().AsMemory();
        PasswordMemoryChar = Password.ToCharArray().AsMemory();
        PhoneNumberMemoryChar = PhoneNumber.ToCharArray().AsMemory();
    }
}