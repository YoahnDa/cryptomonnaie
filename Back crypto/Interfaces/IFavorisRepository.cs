using Backend_Crypto.Data;
using Backend_Crypto.Models;

namespace Backend_Crypto.Interfaces
{
    public interface IFavorisRepository
    {
        ICollection<Favoris> GetFavoris(int idUser);
        bool isFavoris(int idCrypto,int idUser);    
        bool CreateFavoris(int idcrypto,int idUser);
        bool removeFavoris(int idCrypto,int idUser);
        bool Save();
    }
}
