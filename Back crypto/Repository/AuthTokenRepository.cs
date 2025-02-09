using Backend_Crypto.Data;
using Backend_Crypto.Interfaces;
using Backend_Crypto.Models;
using Backend_Crypto.Services;

namespace Backend_Crypto.Repository
{
    public class AuthTokenRepository : IAuthTokenRepository
    {
        private readonly ITokenValidator _tokenValidator;
        private readonly EmailProvider _emailProvider;
        private readonly DataContext _dataContext;
        public AuthTokenRepository(ITokenValidator tokenValidator,DataContext context,EmailProvider provider)
        {
            _tokenValidator = tokenValidator;
            _dataContext = context;
            _emailProvider = provider;
        }
        public bool CreateToken(Transaction transac,string username,string email)
        {
            var token = _tokenValidator.GenerateUniqueToken(username);
            AuthToken authToken = new AuthToken() 
            { 
                Token = token,
                Transac = transac
            };
            _dataContext.Add(authToken);
            _emailProvider.SendEmailAutorisationVente(token,email);
            return Save();
        }

        public Transaction GetTransaction(string token)
        {
            var tokens = _dataContext.Tokens.FirstOrDefault(t => t.Token == token);
            return tokens.Transac;
        }

        public bool isTokenExist(string token)
        {
            return _dataContext.Tokens.Any(t => t.Token == token);
        }

        public bool isTokenValid(string token)
        {
            return _dataContext.Tokens.Any(t => t.Token == token && !t.IsUsed);
        }

        public bool Save()
        {
            var saved = _dataContext.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
