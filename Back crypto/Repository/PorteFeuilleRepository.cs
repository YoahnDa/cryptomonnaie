using AutoMapper;
using AutoMapper.QueryableExtensions;
using Backend_Crypto.Data;
using Backend_Crypto.Dto;
using Backend_Crypto.Interfaces;
using Backend_Crypto.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend_Crypto.Repository
{
    public class PorteFeuilleRepository : IPorteFeuilleRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public PorteFeuilleRepository(DataContext dataContext , IMapper mapper)
        {
            _context = dataContext;
            _mapper = mapper;
        }
        public bool AddStockPortefeuille(Crypto crypto , Portefeuille portefeuille , double stock = 0)
        {
            StockPortefeuille stockPortefeuille = new StockPortefeuille() 
            {
                CryptoIn = crypto,
                Stock = stock
            };
            portefeuille.Stock.Add(stockPortefeuille);
            _context.Update(portefeuille);
            return Save();
        }

        public bool CreatePortefeuille(int idUser)
        {
            Portefeuille portefeuille = new Portefeuille() 
            {
                IdUser = idUser,
            };
            _context.Add(portefeuille);
            return Save();
        }

        public bool ExchangeFond(Portefeuille porte, double fond, bool isRetrait)
        {
            porte.Fond = isRetrait ? porte.Fond - fond : porte.Fond + fond;
            _context.Update(porte);
            return Save();
        }

        public Portefeuille? GetPortefeuille(int idUser)
        {
            return _context.Portefeuilles.FirstOrDefault(c => c.IdUser == idUser);
        }

        public bool HaveCrypto(int idUser,int idCrypto)
        {
            var portefeuille = GetPortefeuille(idUser);
            return portefeuille!=null && portefeuille.Stock.Any(c => c.IdCrypto == idCrypto);
        }

        public bool HaveEnoughCrypto(int idUser, int idCrypto, double need)
        {
            var portefeuille = GetPortefeuille(idUser);

            // Trouver la crypto dans le portefeuille
            StockPortefeuille stock = portefeuille.Stock.FirstOrDefault(c => c.IdCrypto == idCrypto);

            // Vérifier si la crypto existe et si le solde est suffisant
            return stock != null && stock.Stock >= need;
        }

        public StockPortefeuille getStock(int idUser,int idCrypto)
        {
            var portefeuille = GetPortefeuille(idUser);
            return portefeuille.Stock.FirstOrDefault(c => c.IdCrypto == idCrypto);
        }


        public bool HaveEnoughFond(int idUser , double need)
        {
            Portefeuille portefeuille = GetPortefeuille(idUser);
            return portefeuille != null && portefeuille.Fond >= need;
        }

        public bool PortefeuilleExiste(int idUser)
        {
            return _context.Portefeuilles.Any(p => p.IdUser == idUser);
        }

        public bool RemovePortefeuille(Portefeuille portefeuille)
        {
            _context.Remove(portefeuille);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdatePortefeuille(Portefeuille portefeuille)
        {
            _context.Update(portefeuille);
            return Save();
        }
    }
}
