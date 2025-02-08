using Backend_Crypto.Data;
using Backend_Crypto.Dto;
using Backend_Crypto.Interfaces;
using Backend_Crypto.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;

namespace Backend_Crypto.Services
{
    public class UserAnalytique
    {
        private readonly IPorteFeuilleRepository _porteFeuilleRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ExternalApiService _externalApiService;
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

        public async Task<List<TransactionDtoAdmin>> getTransac(List<TypeTransaction> type,string token)
        {
            var transactions = new List<TransactionDtoAdmin>();
            var listTransac = _transactionRepository.GetTransactionByTypes(type);
            foreach (var transaction in listTransac)
            {
                JObject user = await _externalApiService.GetDataFromApiAsync("admin/userinfo/" + transaction.PortefeuilleOwner.IdUser, token);
                transactions.Add(new TransactionDtoAdmin()
                {
                    IdTransaction = transaction.IdTransaction,
                    Type = Enum.GetName(typeof(TypeTransaction), transaction.Type),
                    State = Enum.GetName(typeof(Status), transaction.State),
                    DateTransaction = transaction.DateTransaction,
                    idUser = transaction.PortefeuilleOwner.IdUser,
                    fond = transaction.fond,
                    nom = user["username"].ToString()
                });
            }
            return transactions;
        }

        public async Task<List<TransactionDtoAdmin>> getTransacUser(List<TypeTransaction> type,int idPortefeuille,string token,bool isAdmin = false)
        {
            var transactions = new List<TransactionDtoAdmin>();
            var listTransac = _transactionRepository.GetTransactionByTypesPortefeuille(type,idPortefeuille);
            foreach (var transaction in listTransac)
            {
                JObject user = isAdmin ? await _externalApiService.GetDataFromApiAsync("admin/userinfo/"+transaction.PortefeuilleOwner.IdUser,token) : await _externalApiService.GetDataFromApiAsync("admin/user",token);
                transactions.Add(new TransactionDtoAdmin()
                {
                    IdTransaction = transaction.IdTransaction,
                    Type = Enum.GetName(typeof(TypeTransaction), transaction.Type),
                    State = Enum.GetName(typeof(Status), transaction.State),
                    DateTransaction = transaction.DateTransaction,
                    idUser = transaction.PortefeuilleOwner.IdUser,
                    fond = transaction.fond,
                    nom = user["username"].ToString()
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

        public async Task<List<TransactionDtoAdmin>> GetTransactionsByFilter(TransactionFilterDto filter,string token)
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
                JObject user = await _externalApiService.GetDataFromApiAsync("admin/userinfo/" + transaction.PortefeuilleOwner.IdUser, token);
                transactions.Add(new TransactionDtoAdmin()
                {
                    IdTransaction = transaction.IdTransaction,
                    Type = Enum.GetName(typeof(TypeTransaction), transaction.Type),
                    State = Enum.GetName(typeof(Status), transaction.State),
                    DateTransaction = transaction.DateTransaction,
                    idUser = transaction.PortefeuilleOwner.IdUser,
                    fond = transaction.fond,
                    nom = user["username"].ToString()
                });
            }

            return transactions;
        }

        public async Task<AnalysePortefeuilleDto> getInfoPortefeuille(int idUser,String token )
        {
            var portefeuille = _porteFeuilleRepository.GetPortefeuille(idUser);
            return new AnalysePortefeuilleDto()
            {
                IdPortefeuille = portefeuille.IdPortefeuille,
                Fond = portefeuille.Fond,
                listeEchange = await getTransacUser(new List<TypeTransaction>() { TypeTransaction.Retrait, TypeTransaction.Depot }, portefeuille.IdPortefeuille,token,false),
                listeOperation = await getTransacUser(new List<TypeTransaction>() { TypeTransaction.Vente, TypeTransaction.Achat }, portefeuille.IdPortefeuille,token ,false)
            };
        }
    }
}
