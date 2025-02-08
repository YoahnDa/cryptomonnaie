using Backend_Crypto.Data;
using Backend_Crypto.Interfaces;
using Backend_Crypto.Models;

namespace Backend_Crypto.Repository
{
    public class FavorisRepository : IFavorisRepository
    {   
        private readonly DataContext _dataContext;
        private readonly ICryptoRepository _cryptoRepository;

        public FavorisRepository(DataContext context,ICryptoRepository crypto) 
        {
            _dataContext = context;
            _cryptoRepository = crypto;
        }

        public bool CreateFavoris(int idcrypto, int idUser)
        {
            var crypto = _cryptoRepository.GetCrypto(idcrypto);
            var favoris = new Favoris()
            {
                idUser = idUser,
                Cryptos = crypto
            };
            _dataContext.Add(favoris);
            return Save();
        }

        public ICollection<Favoris> GetFavoris(int idUser)
        {
            return _dataContext.Favoris
                                .Where(f => f.idUser == idUser)
                                .ToList();
        }

        public bool isFavoris(int idCrypto, int idUser)
        {
            return _dataContext.Favoris.Any(f => f.idUser == idUser && f.idCrypto == idCrypto);
        }

        public bool removeFavoris(int idCrypto, int idUser)
        {
            var favs= _dataContext.Favoris.FirstOrDefault(f => f.idUser == idUser && f.idCrypto ==idCrypto);
            _dataContext.Remove(favs);
            return Save();
        }

        public bool Save()
        {
            var saved = _dataContext.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
