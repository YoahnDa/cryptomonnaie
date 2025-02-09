namespace Backend_Crypto.Interfaces
{
    public interface ITokenValidator
    {
        string? GetTokenFromHeader();
        string GenerateUniqueToken(string username);
    }
}
