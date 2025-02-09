using Backend_Crypto.Data;
using Backend_Crypto.Interfaces;
using Backend_Crypto.Models;

namespace Backend_Crypto.Repository
{
    public class StockPortefeuilleRepository : IStockPortefeuilleRepository
    {
        private readonly DataContext _context;

        public StockPortefeuilleRepository(DataContext context)
        {
            _context = context;
        }

        public bool addStock(StockPortefeuille stock)
        {
            _context.Add(stock);
            return Save();
        }

        public bool removeStock(StockPortefeuille stock)
        {
            _context.Remove(stock);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool updateStock(StockPortefeuille stock)
        {
            _context.Update(stock);
            return Save();
        }
    }
}
