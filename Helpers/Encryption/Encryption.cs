using System.Security.Cryptography;
using System.Text;
using alderaan_shop.Helpers.CustomExceptions;
using Konscious.Security.Cryptography;

namespace alderaan_shop.Helpers.Encryption;

public class Encryption
{
    private static readonly byte[] AppEncryptKey = Convert.FromBase64String("4q3J8v9dG7sL2hP0yK5nVzXqT1wAeF8mYcQpR2jL6uM=");
    private static readonly byte[] AppHmacKey = Convert.FromBase64String("4q3J8v9dG7sL2hP0yK5nVzXqT1wAeF8mYcQpR2jL6uM=");
    private static readonly SemaphoreSlim Semaphore = new(Environment.ProcessorCount / 2);

    public static async Task<Memory<char>> HashPassword(Memory<char> password)
    {
        await Semaphore.WaitAsync();
        try
        {
            // Random generation of a 32-bytes salt stored alongside the hashed password as an additional security layer :
            // 1. Argon2id hashing is deterministic
            //      - Two exact same inputs will have the same hashed output
            // 2. Adding a random salt (recommended minimum is 16 bytes) ensures that every hash is unique, even if multiple users have the same password
            // 3. Prevents the use of Rainbow Tables in case of data theft
            //      - Attacker rely on precomputed hash tables to quickly compare common passwords to stolen hashes
            //      - A random salt invalidates these tables by making such comparisons computationally infeasible
            //      - "Computationally infeasible" because : 32 bytes (256 bits) equals 2^256 possibilities
            //      - ≈ 115.8 quattuorvigintillion (≈ 1.158 * 10^77) possibilities
            //      - Even a 16 bytes salt makes Rainbow Tables completely obsolete
            byte[] salt = new byte[32];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // "using" ensures that this instance of Argon2id will be disposed of properly
            using (Argon2id argon2 = new Argon2id(Encoding.UTF8.GetBytes(password.ToString())))
            {
                argon2.Salt = salt;
                argon2.Iterations = 3;
                argon2.MemorySize = 64 * 1024;
                argon2.DegreeOfParallelism = 2;

                // Convert the data to a 32 bytes array before comparison
                byte[] hash = await argon2.GetBytesAsync(32);

                // A result of 64 bytes total (Salt 32 + Hash 32)
                byte[] result = new byte[salt.Length + hash.Length];
                // Cutting the salt and hash before pasting it again at the correct place to ensure correct data format
                Buffer.BlockCopy(salt, 0, result, 0, salt.Length);
                Buffer.BlockCopy(hash, 0, result, salt.Length, hash.Length);

                
                return Convert.ToBase64String(result).ToCharArray().AsMemory();
            }
        }
        catch (Exception)
        {
            throw new PasswordHashException("Une erreur interne s'est produite lors de la vérification/l'encryption du mot de passe. Veuillez s'il vous plaît contacter un administrateur.");
        }
        finally
        {
            // Using finally so after the try/catch is done, SemaphoreSlim informs the system that the core it was using is now free
            Semaphore.Release();
        }
    }

    public static async Task<bool> VerifyPassword(Memory<char> password, string storedHash)
    {
        await Semaphore.WaitAsync();
        try
        {
            byte[] combined = Convert.FromBase64String(storedHash);

            ReadOnlySpan<byte> combinedSpan = combined;
            ReadOnlySpan<byte> salt = combinedSpan.Slice(0, 32);
            byte[] originalHash = combinedSpan.Slice(32, 32).ToArray();

            using Argon2id argon2 = new Argon2id(Encoding.UTF8.GetBytes(password.Span.ToString()));
            argon2.Salt = salt.ToArray();
            argon2.Iterations = 3;
            argon2.MemorySize = 64 * 1024; // 256 MiB
            argon2.DegreeOfParallelism = 2;

            // Convert the data to a 32 bytes array before comparison
            byte[] computedHash = await argon2.GetBytesAsync(32);
            
            // FixedTimeEquals negates Oracle security failures :
            // 1. Without it, a malicious user can deduct the correct required characters through response timing
            // 2. It keeps on comparing the two data even if it finds a difference, to avoid giving insight
            return CryptographicOperations.FixedTimeEquals(originalHash, computedHash);
        }
        catch (Exception ex) when (ex is WrongCredentialsException)
        {
            throw;
        }
        catch (Exception)
        {
            throw new PasswordHashException("Une erreur interne s'est produite lors du processus de vérification de votre mot de passe. Si le problème persiste, veuillez s'il vous plaît contacter un administrateur.");
        }
        finally
        {
            Semaphore.Release();
        }
    }

    public static Memory<char> CipherData(Memory<char> cipheredText)
    {
        if (AppEncryptKey.Length != 32) throw new InvalidOperationException("Le format de la clé de chiffrage est incorrect. Veuillez s'il vous plaît contacter un administrateur.");

        using AesGcm aes = new AesGcm(AppEncryptKey, tagSizeInBytes: 16);

        byte[] textBytes = Encoding.UTF8.GetBytes(cipheredText.Span.ToString());
        byte[] combined = new byte[12 + 16 + textBytes.Length];

        // Using a Span<byte> to avoid unnecessary memory allocation :
        // 1. Similar in concept to C pointers by providing a view over data in memory without copying it
        // 2. Allows this method to modify "combined" without creating new buffers or copies
        // 3. Span<T> represents a contiguous region of memory, so Span<byte[]> would be incorrect
        //      - We don't want a span of arrays, but a span of bytes.
        // 4. Span<T> is used over Memory<T> here because we don't need these values to persist outside of this method's scope
        Span<byte> combinedSpan = combined;
        Span<byte> iv = combinedSpan.Slice(0, 12);
        Span<byte> tag = combinedSpan.Slice(12, 16);
        Span<byte> cipherText = combinedSpan.Slice(28);

        RandomNumberGenerator.Fill(iv);

        // Modifies "combined" directly through the pointers/views (Span<byte>)
        aes.Encrypt(iv, textBytes.ToArray(), cipherText, tag);

        return Convert.ToBase64String(combined).ToCharArray().AsMemory();
    }

    public static string DecipherData(string encoded)
    {
        using AesGcm aes = new AesGcm(AppEncryptKey, tagSizeInBytes: 16);

        byte[] combined = Convert.FromBase64String(encoded);
        ReadOnlySpan<byte> combinedSpan = combined;
        ReadOnlySpan<byte> iv = combinedSpan.Slice(0, 12);
        ReadOnlySpan<byte> tag = combinedSpan.Slice(12, 16);
        ReadOnlySpan<byte> ciphertext = combinedSpan.Slice(28);

        byte[] plaintextBytes = new byte[ciphertext.Length];
        aes.Decrypt(iv, ciphertext, tag, plaintextBytes);

        return Encoding.UTF8.GetString(plaintextBytes);
    }

    public static Memory<char> ComputeUniqueHmac(Memory<char> data)
    {
        byte[] hmacKey = AppHmacKey ?? throw new ArgumentNullException("Clé HMAC introuvable. Veuillez s'il vous plaît contacter un administrateur.");

        using HMACSHA256 hmac = new HMACSHA256(hmacKey);

        return Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(data.ToArray()))).ToCharArray().AsMemory();
    }
}