using Backend_Crypto.Interfaces;

namespace Backend_Crypto.Services
{
    public class TokenValidator : ITokenValidator
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

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
    }
}
