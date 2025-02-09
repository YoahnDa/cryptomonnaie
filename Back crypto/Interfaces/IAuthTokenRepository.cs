using Backend_Crypto.Data;
using Backend_Crypto.Models;

namespace Backend_Crypto.Interfaces
{
    public interface IAuthTokenRepository
    {
        Transaction GetTransaction(string token);
        bool isTokenExist(string token);
        bool isTokenValid(string token);
        bool CreateToken(Transaction transac,string username,string email);
        bool Save();
    }
}
