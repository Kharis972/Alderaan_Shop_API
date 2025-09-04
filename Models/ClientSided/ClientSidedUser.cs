namespace alderaan_shop.Models.ClientSided;

//1. Renvoi uniquement les données user que le user va voir sur le front (pour éviter l'overfetch) !
//2. Ces données vont être send dans un cookie qui n'est pas HTTPOnly(Request Only) !
//3. Pour être manipulable en JS dans un cookie pour l'afficher sur le front !
//4. Dans le cas ou le cookie est modifié ça n'aura impacte sur le server/API !
public class ClientSidedUser
(
    string firstName,
    string lastName,
    string address, 
    string zipCode,
    string mail,
    string phoneNumber
)
{
    public string FirstName { get; init; } = firstName;
    public string LastName { get; init; } = lastName;
    public string Address { get; init; } = address;
    public string ZipCode { get; init; } = zipCode;
    public string Mail { get; init; } = mail;
    public string PhoneNumber { get; init; } = phoneNumber;
}