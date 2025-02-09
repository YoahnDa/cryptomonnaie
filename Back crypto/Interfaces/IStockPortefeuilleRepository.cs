using Backend_Crypto.Data;
using Backend_Crypto.Models;

namespace Backend_Crypto.Interfaces
{
    public interface IStockPortefeuilleRepository
    {
        bool updateStock(StockPortefeuille stock);
        bool removeStock(StockPortefeuille stock);
        bool addStock(StockPortefeuille stock);
        bool Save();
    }
}
