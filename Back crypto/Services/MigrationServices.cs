using AutoMapper;
using Backend_Crypto.Data;
using Backend_Crypto.Dto;
using Backend_Crypto.Helper;
using Backend_Crypto.Interfaces;
using Google.Cloud.Firestore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Backend_Crypto.Services
{
    public class MigrationServices
    {
        private readonly FirestoreDb _firestore;
        private readonly DataContext _dbContext;
        private readonly ICryptoRepository _cryptoRepository;
        private readonly IMapper _mapper;

        public MigrationServices(DataContext dbContext,IMapper mapper, ICryptoRepository cryptoRepository)
        {
            // Remplacez "TON_PROJET_FIREBASE" par l'ID de votre projet Firebase
            _firestore = FirestoreDb.Create("testfirebaseproject-8cc5e");
            _dbContext = dbContext;
            _mapper = mapper;
            _cryptoRepository = cryptoRepository;
        }

        public async Task MigrateDataIfNeeded()
        {
            await MigrateCryptosAsync();
            await MigrationFavorisAsync();
            await MigrationTransactionssAsync();
            await MigrationPorteFeuillesAsync();
            await MigrationOrdresAsync();
            await MigrationStockPortefeuilleAsync();
            await MigrationHistoriqueAsync();
            // Ajoutez d'autres méthodes de migration pour chaque entité
        }

        private async Task MigrateCryptosAsync()
        {
            var cryptoRef = _firestore.Collection("cryptos");
            var snapshot = await cryptoRef.GetSnapshotAsync();

            if (!snapshot.Documents.Any())
            {

                Console.WriteLine("Migration des utilisateurs en cours...");
                var cryptos = await _dbContext.Cryptos.ToListAsync();
                var cryptoDtos = _mapper.Map<List<CryptoFirebaseDto>>(cryptos);

                foreach (var cryptoDto in cryptoDtos)
                {
                    var docRef = cryptoRef.Document(cryptoDto.IdCrypto.ToString());

                    await docRef.SetAsync(new { 
                        cryptoDto.IdCrypto,
                        cryptoDto.Nom,
                        cryptoDto.Symbole,
                        prix = _cryptoRepository.GetPrixCrypto(cryptoDto.IdCrypto)
                    });
                }
                Console.WriteLine("Migration des utilisateurs terminée !");
            }
        }

        private async Task MigrationHistoriqueAsync()
        {
            var transactionsRef = _firestore.Collection("historiquesprix");
            var snapshot = await transactionsRef.GetSnapshotAsync();

            if (!snapshot.Documents.Any())
            {
                Console.WriteLine("Migration des historique en cours...");
                var historiques = await _dbContext.Historiques.ToListAsync();
                var historiqueDtos = _mapper.Map<List<HistoriquePrixFirebaseDto>>(historiques);

                foreach (var historiqueDto in historiqueDtos)
                {
                    var docRef = transactionsRef.Document(historiqueDto.IdHistorique.ToString());
                    await docRef.SetAsync(new {
                        historiqueDto.IdHistorique,
                        historiqueDto.PrixCrypto,
                        historiqueDto.DateChange,
                        historiqueDto.idCrypto
                    });
                }
                Console.WriteLine("Migration des historique terminée !");
            }
        }

        private async Task MigrationFavorisAsync()
        {
            var transactionsRef = _firestore.Collection("favoris");
            var snapshot = await transactionsRef.GetSnapshotAsync();

            if (!snapshot.Documents.Any())
            {
                Console.WriteLine("Migration des favoris en cours...");
                var favoris = await _dbContext.Favoris.ToListAsync();
                var favoriDtos = _mapper.Map<List<FavorisDto>>(favoris);

                foreach (var favoriDto in favoriDtos)
                {
                    var docRef = transactionsRef.Document(favoriDto.IdFavoris.ToString());
                    await docRef.SetAsync(new
                    {
                        favoriDto.IdFavoris,
                        favoriDto.idCrypto,
                        favoriDto.idUser
                    });
                }
                Console.WriteLine("Migration des favoris terminée !");
            }
        }

        private async Task MigrationTransactionssAsync()
        {
            var transactionsRef = _firestore.Collection("transactions");
            var snapshot = await transactionsRef.GetSnapshotAsync();

            if (!snapshot.Documents.Any())
            {
                Console.WriteLine("Migration des transaction en cours...");
                var transaction = await _dbContext.Transac.ToListAsync();
                var transactionDtos = _mapper.Map<List<TransactionFirebaseDto>>(transaction);

                foreach (var transactionDto in transactionDtos)
                {
                    var docRef = transactionsRef.Document(transactionDto.IdTransaction.ToString());
                    await docRef.SetAsync(new
                    {
                        transactionDto.IdTransaction,
                        transactionDto.fond,
                        transactionDto.DateTransaction,
                        transactionDto.IdPortefeuille,
                        State = transactionDto.State.ToString(),
                        Type = transactionDto.Type.ToString()
                    });
                }
                Console.WriteLine("Migration des transactions terminée !");
            }
        }

        private async Task MigrationPorteFeuillesAsync()
        {
            var transactionsRef = _firestore.Collection("portefeuilles");
            var snapshot = await transactionsRef.GetSnapshotAsync();

            if (!snapshot.Documents.Any())
            {
                Console.WriteLine("Migration des portefeuilles en cours...");
                var portefeuille = await _dbContext.Portefeuilles.ToListAsync();
                var portefeuilleDtos = _mapper.Map<List<PortefeuilleFirebaseDto>>(portefeuille);

                foreach (var portefeuilleDto in portefeuilleDtos)
                {
                    var docRef = transactionsRef.Document(portefeuilleDto.IdPortefeuille.ToString());
                    await docRef.SetAsync(new
                    {
                        portefeuilleDto.IdPortefeuille,
                        portefeuilleDto.Fond,
                        portefeuilleDto.IdUser
                    });
                }
                Console.WriteLine("Migration des portefeuilles terminée !");
            }
        }

        private async Task MigrationOrdresAsync()
        {
            var transactionsRef = _firestore.Collection("ordres");
            var snapshot = await transactionsRef.GetSnapshotAsync();

            if (!snapshot.Documents.Any())
            {
                Console.WriteLine("Migration des ordres en cours...");
                var ordres = await _dbContext.Ordres.ToListAsync();
                var ordreDtos = _mapper.Map<List<OrdreFirebaseDto>>(ordres);

                foreach (var ordreDto in ordreDtos)
                {
                    var docRef = transactionsRef.Document(ordreDto.IdOrdre.ToString());
                    await docRef.SetAsync(new
                    {
                        ordreDto.IdOrdre,
                        ordreDto.AmountCrypto,
                        ordreDto.IdCrypto,
                        ordreDto.PrixUnitaire,
                        ordreDto.IdTransaction
                    });
                }
                Console.WriteLine("Migration des ordres terminée !");
            }
        }

        private async Task MigrationStockPortefeuilleAsync()
        {
            var transactionsRef = _firestore.Collection("stockPortefeuille");
            var snapshot = await transactionsRef.GetSnapshotAsync();

            if (!snapshot.Documents.Any())
            {
                Console.WriteLine("Migration des stocks en cours...");
                var stocks = await _dbContext.Stocks.ToListAsync();
                var stockDtos = _mapper.Map<List<StockFirebaseDto>>(stocks);

                foreach (var stockDto in stockDtos)
                {
                    var docRef = transactionsRef.Document(stockDto.IdStock.ToString());
                    await docRef.SetAsync(new
                    {
                        stockDto.IdStock,
                        stockDto.IdCrypto,
                        stockDto.Stock,
                        stockDto.PorteFeuilleOwn.IdPortefeuille
                    });
                }
                Console.WriteLine("Migration des stocks terminée !");
            }
        }
    }
}
