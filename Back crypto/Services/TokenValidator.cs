using Backend_Crypto.Interfaces;
using System.Security.Cryptography;

namespace Backend_Crypto.Services
{
    public class TokenValidator : ITokenValidator
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _secretKey = "cryptomonnaie";

        public TokenValidator(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string? GetTokenFromHeader()
        {
            // Récupérer le token du header Authorization
            var authorizationHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();

            // Vérifier si l'en-tête existe et si c'est un Bearer Token
            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer ", StringComparison.InvariantCultureIgnoreCase))
            {
                return authorizationHeader.Substring("Bearer ".Length).Trim();
            }

            // Si le token n'existe pas ou est mal formé, renvoyer null ou une exception
            return null; // Ou vous pouvez lancer une exception si nécessaire
        }

        public string GenerateUniqueToken(string username)
        {
            using (var hmac = new HMACSHA256(System.Text.Encoding.UTF8.GetBytes(_secretKey)))
            {
                // Crée un "hash" basé sur le nom d'utilisateur et un timestamp pour assurer l'unicité
                var data = $"{username}-{DateTime.UtcNow.Ticks}";
                var tokenBytes = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(data));
                return Convert.ToBase64String(tokenBytes);
            }
        }
    }
}
