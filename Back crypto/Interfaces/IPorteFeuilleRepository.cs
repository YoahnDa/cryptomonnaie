using Backend_Crypto.Dto;
using Backend_Crypto.Models;

namespace Backend_Crypto.Interfaces
{
    public interface IPorteFeuilleRepository
    {
        Portefeuille? GetPortefeuille(int idUser);
        bool PortefeuilleExiste(int idUser);
        bool CreatePortefeuille(int idUser);
        bool RemovePortefeuille(Portefeuille portefeuille);
        bool AddStockPortefeuille(Crypto crypto , Portefeuille portefeuille , double stock);
        bool HaveEnoughFond(int idUser , double need);
        bool HaveCrypto(int idUser,int idCrypto);
        bool HaveEnoughCrypto(int idUser,int idCrypto,double  need);
        bool UpdatePortefeuille(Portefeuille portefeuille);
        StockPortefeuille getStock(int idUser, int idCrypto);
        bool ExchangeFond(Portefeuille porte, double fond, bool isRetrait);
        bool Save();
    }
}
