using Backend_Crypto.Data;
using Backend_Crypto.Dto;
using Backend_Crypto.Interfaces;
using Backend_Crypto.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Backend_Crypto.Services
{
    public class UserAnalytique
    {
        private readonly IPorteFeuilleRepository _porteFeuilleRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IFavorisRepository _favorisRepository;
        private readonly ICryptoRepository _cryptoRepository;
        private readonly DataContext _context;

        public UserAnalytique(IPorteFeuilleRepository porteFeuilleRepository, ITransactionRepository transactionRepository, ICryptoRepository cryptoRepository,IFavorisRepository favorisRepository, DataContext context)
        {
            _porteFeuilleRepository = porteFeuilleRepository;
            _transactionRepository = transactionRepository;
            _cryptoRepository = cryptoRepository;
            _favorisRepository = favorisRepository;
            _context = context;
        }

        public List<TransactionDtoAdmin> getTransac(List<TypeTransaction> type)
        {
            var transactions = new List<TransactionDtoAdmin>();
            var listTransac = _transactionRepository.GetTransactionByTypes(type);
            foreach (var transaction in listTransac)
            {
                transactions.Add(new TransactionDtoAdmin()
                {
                    IdTransaction = transaction.IdTransaction,
                    Type = Enum.GetName(typeof(TypeTransaction), transaction.Type),
                    State = Enum.GetName(typeof(Status), transaction.State),
                    DateTransaction = transaction.DateTransaction,
                    idUser = transaction.PortefeuilleOwner.IdUser,
                    fond = transaction.fond
                });
            }
            return transactions;
        }

        public List<TransactionDtoAdmin> getTransacUser(List<TypeTransaction> type,int idPortefeuille)
        {
            var transactions = new List<TransactionDtoAdmin>();
            var listTransac = _transactionRepository.GetTransactionByTypesPortefeuille(type,idPortefeuille);
            foreach (var transaction in listTransac)
            {
                transactions.Add(new TransactionDtoAdmin()
                {
                    IdTransaction = transaction.IdTransaction,
                    Type = Enum.GetName(typeof(TypeTransaction), transaction.Type),
                    State = Enum.GetName(typeof(Status), transaction.State),
                    DateTransaction = transaction.DateTransaction,
                    idUser = transaction.PortefeuilleOwner.IdUser,
                    fond = transaction.fond
                });
            }
            return transactions;
        }

        public bool updateFavs(int idUser,int idCrypto)
        {
            if(_favorisRepository.isFavoris(idUser, idCrypto))
            {
                return _favorisRepository.removeFavoris(idCrypto, idUser);
            }
            else
            {
                return _favorisRepository.CreateFavoris(idCrypto, idUser);
            }
        }

        public List<TransactionDtoAdmin> GetTransactionsByFilter(TransactionFilterDto filter)
        {
            var query = _context.Transac.AsQueryable();

            if (filter.Types != null && filter.Types.Any())
            {
                query = query.Where(t => filter.Types.Contains(t.Type));
            }

            if (filter.IdUtilisateurs != null && filter.IdUtilisateurs.Any())
            {
                query = query.Where(t => filter.IdUtilisateurs.Contains(t.PortefeuilleOwner.IdUser));
            }

            if (filter.IdCryptos != null && filter.IdCryptos.Any())
            {
                query = query.Where(t => t.Ordre != null && filter.IdCryptos.Contains(t.Ordre.IdCrypto));
            }

            var transactions = new List<TransactionDtoAdmin>();
            var listTransac = query.ToList(); // Exécute la requête pour obtenir la liste filtrée

            foreach (var transaction in listTransac)
            {
                transactions.Add(new TransactionDtoAdmin()
                {
                    IdTransaction = transaction.IdTransaction,
                    Type = Enum.GetName(typeof(TypeTransaction), transaction.Type),
                    State = Enum.GetName(typeof(Status), transaction.State),
                    DateTransaction = transaction.DateTransaction,
                    idUser = transaction.PortefeuilleOwner.IdUser,
                    fond = transaction.fond
                });
            }

            return transactions;
        }

        public AnalysePortefeuilleDto getInfoPortefeuille(int idUser)
        {
            var portefeuille = _porteFeuilleRepository.GetPortefeuille(idUser);
            return new AnalysePortefeuilleDto()
            {
                IdPortefeuille = portefeuille.IdPortefeuille,
                Fond = portefeuille.Fond,
                listeEchange = getTransacUser(new List<TypeTransaction>() { TypeTransaction.Retrait, TypeTransaction.Depot }, portefeuille.IdPortefeuille),
                listeOperation = getTransacUser(new List<TypeTransaction>() { TypeTransaction.Vente, TypeTransaction.Achat }, portefeuille.IdPortefeuille)
            };
        }
    }
}
