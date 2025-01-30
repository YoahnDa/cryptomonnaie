using Backend_Crypto.Dto;
using Backend_Crypto.Models;

namespace Backend_Crypto.Interfaces
{
    public interface IPorteFeuilleRepository
    {
        PortefeuilleDto? GetPortefeuille(int idUser);
        bool PortefeuilleExiste(int idUser);
        bool CreatePortefeuille(int idUser);
        bool RemovePortefeuille(Portefeuille portefeuille);
        bool AddStockPortefeuille(Crypto crypto , Portefeuille portefeuille , double stock);
        bool HaveEnoughFond(int idUser , double need);
        bool HaveCrypto(int idUser,int idCrypto);
        bool ExchangeFond(Portefeuille porte, double fond, bool isRetrait);
        bool Save();
    }
}
