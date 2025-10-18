using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace alderaan_shop.DTOs.Product;

public class NewProductDTO
{
    [Required(ErrorMessage = "Le nom est obligatoire pour créer un compte.")]
    [MinLength(2, ErrorMessage = "Le nom doit contenir au minimum 2 caractères.")]
    [MaxLength(50, ErrorMessage = "Le nom doit contenir au maximum 50 caractères.")]
    [RegularExpression(@"^[A-Za-zÀ-ÖØ-öø-ÿ' -]{2,50}$",
        ErrorMessage = "Le nom ne doit contenir que des lettres, et faire entre 2 et 50 caractères.")]

    public required string Name { get; init; }
    
    [Required(ErrorMessage = "La description est obligatoire.")]
    [MinLength(10, ErrorMessage = "La description doit contenir au minimum 10 caractères.")]
    [MaxLength(500, ErrorMessage = "La description doit contenir au maximum 500 caractères.")]
    [RegularExpression(@"^[A-Za-zÀ-ÖØ-öø-ÿ0-9'.,;:!?()\- ]{10,500}$",
        ErrorMessage = "La description ne doit contenir que des lettres, chiffres et ponctuations simples, entre 10 et 500 caractères.")]
    public required string Description { get; init; }
    
    [Required(ErrorMessage = "Le prix est obligatoire.")]
    [Range(0.01, 10000.00, ErrorMessage = "Le prix doit être compris entre 0,01 € et 10 000,00 €.")]
    public required decimal Price { get; init; }
    
    [Required(ErrorMessage = "L'URL de l'image est obligatoire.")]
    [MaxLength(300, ErrorMessage = "L'URL de l'image ne doit pas dépasser 300 caractères.")]
    [RegularExpression(@"^(https?:\/\/.*\.(png|jpg|jpeg|gif|webp))(\?.*)?(#.*)?$", 
        ErrorMessage = "L'URL doit être valide et se terminer par .png, .jpg, .jpeg, .gif ou .webp.")]

    public required string ImageUrl { get; init; }
    
    [Required(ErrorMessage = "La quantité en stock est obligatoire.")]
    [Range(0, 10000, ErrorMessage = "La quantité en stock doit être comprise entre 0 et 10 000.")]
    public required int Stock { get; init; }

    [NotMapped] 
    public Memory<char> NameMemoryChar;
    [NotMapped] 
    public Memory<char> DescriptionMemoryChar;
    [NotMapped] 
    public Memory<char> PriceMemoryChar;
    [NotMapped] 
    public Memory<char> ImageUrlMemoryChar;
    [NotMapped] 
    public Memory<char> StockMemoryChar;
    
    public void InitializeMemory()
    {
        NameMemoryChar = Name.ToCharArray().AsMemory();
        DescriptionMemoryChar = Description.ToCharArray().AsMemory();
        PriceMemoryChar = Price.ToString().ToCharArray().AsMemory();
        ImageUrlMemoryChar = ImageUrl.ToCharArray().AsMemory();
        StockMemoryChar = Stock.ToString().ToCharArray().AsMemory();
    }
}