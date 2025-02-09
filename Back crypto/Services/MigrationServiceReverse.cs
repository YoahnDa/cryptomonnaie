using AutoMapper;
using Backend_Crypto.Data;
using Backend_Crypto.Dto;
using Backend_Crypto.Helper;
using Backend_Crypto.Interfaces;
using Backend_Crypto.Models;
using Google.Cloud.Firestore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace Backend_Crypto.Services
{
    public class MigrationServiceReverse
    {
        private readonly FirestoreDb _firestore;
        private readonly DataContext _dbContext;
        private readonly IMapper _mapper;

        public MigrationServiceReverse(DataContext dbContext, IMapper mapper)
        {
            _firestore = FirestoreDb.Create("testfirebaseproject-8cc5e");
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task MigrateDataFromFirebaseAsync()
        {
            await MigrateCryptosAsync();
            await MigrateFavorisAsync();
            await MigrateTransactionsAsync();
            await MigratePortefeuillesAsync();
            await MigrateOrdresAsync();
            await MigrateStockPortefeuilleAsync();
            await MigrateHistoriqueAsync();
        }

        private async Task MigrateCryptosAsync()
        {
            var cryptoRef = _firestore.Collection("cryptos");
            var snapshot = await cryptoRef.GetSnapshotAsync();

            if (snapshot.Documents.Any())
            {
                Console.WriteLine("Migration des cryptos en cours...");
                foreach (var document in snapshot.Documents)
                {
                    var data = document.ToDictionary();

                    var existingTransaction = await _dbContext.Cryptos
                        .FirstOrDefaultAsync(t => t.IdCrypto == Convert.ToInt32(data["IdCrypto"]));

                    if (existingTransaction == null)
                    {
                        var crypto = new Crypto()
                        {
                            IdCrypto = Convert.ToInt32(data["IdCrypto"]),
                            Nom = data["Nom"].ToString(),
                            Symbole = data["Symbole"].ToString()
                        };
                        _dbContext.Cryptos.Add(crypto);
                    }

                }
                await _dbContext.SaveChangesAsync();
                Console.WriteLine("Migration des cryptos terminée !");
            }
        }

        private async Task MigrateFavorisAsync()
        {
            var favorisRef = _firestore.Collection("favoris");
            var snapshot = await favorisRef.GetSnapshotAsync();

            if (snapshot.Documents.Any())
            {
                Console.WriteLine("Migration des favoris en cours...");
                foreach (var document in snapshot.Documents)
                {
                    var favorisDto = document.ConvertTo<FavorisDto>();
                    var favoris = _mapper.Map<Favoris>(favorisDto);

                    var existingFavoris = await _dbContext.Favoris
                        .FirstOrDefaultAsync(f => f.IdFavoris == favoris.IdFavoris);

                    if (existingFavoris == null)
                    {
                        _dbContext.Favoris.Add(favoris);
                    }
                }
                await _dbContext.SaveChangesAsync();
                Console.WriteLine("Migration des favoris terminée !");
            }
        }

        private async Task MigrateTransactionsAsync()
        {
            var transactionsRef = _firestore.Collection("transactions");
            var snapshot = await transactionsRef.GetSnapshotAsync();

            if (snapshot.Documents.Any())
            {
                Console.WriteLine("Migration des transactions en cours...");
                foreach (var document in snapshot.Documents)
                {
                    var data = document.ToDictionary();

                    var existingTransaction = await _dbContext.Transac
                        .FirstOrDefaultAsync(t => t.IdTransaction == Convert.ToInt32(data["IdTransaction"]));

                    if (existingTransaction == null)
                    {
                        var transac = new Models.Transaction()
                        {
                            IdTransaction = Convert.ToInt32(data["IdTransaction"]),
                            fond = Convert.ToDouble(data["fond"]),
                            DateTransaction = Convert.ToDateTime(data["DateTransaction"]),
                            IdPortefeuille = Convert.ToInt32(data["IdPortefeuille"]),
                            State = Enum.Parse<Status>(data["State"].ToString()),
                            Type = Enum.Parse<TypeTransaction>(data["Type"].ToString())
                        };
                        _dbContext.Transac.Add(transac);
                    }
                }
                await _dbContext.SaveChangesAsync();
                Console.WriteLine("Migration des transactions terminée !");
            }
        }

        private async Task MigratePortefeuillesAsync()
        {
            var portefeuillesRef = _firestore.Collection("portefeuilles");
            var snapshot = await portefeuillesRef.GetSnapshotAsync();

            if (snapshot.Documents.Any())
            {
                Console.WriteLine("Migration des portefeuilles en cours...");
                foreach (var document in snapshot.Documents)
                {
                    var portefeuilleDto = document.ConvertTo<PortefeuilleFirebaseDto>();
                    var portefeuille = _mapper.Map<Portefeuille>(portefeuilleDto);

                    var existingPortefeuille = await _dbContext.Portefeuilles
                        .FirstOrDefaultAsync(p => p.IdPortefeuille == portefeuille.IdPortefeuille);

                    if (existingPortefeuille == null)
                    {
                        _dbContext.Portefeuilles.Add(portefeuille);
                    }
                    else
                    {
                        existingPortefeuille.Fond = portefeuille.Fond;
                        existingPortefeuille.IdUser = portefeuille.IdUser;
                    }
                }
                await _dbContext.SaveChangesAsync();
                Console.WriteLine("Migration des portefeuilles terminée !");
            }
        }

        private async Task MigrateOrdresAsync()
        {
            var ordresRef = _firestore.Collection("ordres");
            var snapshot = await ordresRef.GetSnapshotAsync();

            if (snapshot.Documents.Any())
            {
                Console.WriteLine("Migration des ordres en cours...");
                foreach (var document in snapshot.Documents)
                {
                    var ordreDto = document.ConvertTo<OrdreFirebaseDto>();
                    var ordre = _mapper.Map<Ordre>(ordreDto);

                    var existingOrdre = await _dbContext.Ordres
                        .FirstOrDefaultAsync(o => o.IdOrdre == ordre.IdOrdre);

                    if (existingOrdre == null)
                    {
                        _dbContext.Ordres.Add(ordre);
                    }
                    else
                    {
                        existingOrdre.AmountCrypto = ordre.AmountCrypto;
                        existingOrdre.IdCrypto = ordre.IdCrypto;
                        existingOrdre.PrixUnitaire = ordre.PrixUnitaire;
                    }
                }
                await _dbContext.SaveChangesAsync();
                Console.WriteLine("Migration des ordres terminée !");
            }
        }

        private async Task MigrateStockPortefeuilleAsync()
        {
            var stocksRef = _firestore.Collection("stockPortefeuille");
            var snapshot = await stocksRef.GetSnapshotAsync();

            if (snapshot.Documents.Any())
            {
                Console.WriteLine("Migration des stocks en cours...");
                foreach (var document in snapshot.Documents)
                {

                    var data = document.ToDictionary();

                    var existingStock = await _dbContext.Stocks
                        .FirstOrDefaultAsync(s => s.IdStock == Convert.ToInt32(data["IdStock"]));

                    if (existingStock == null)
                    {
                        StockPortefeuille stock = new StockPortefeuille()
                        {
                            IdStock = Convert.ToInt32(data["IdStock"]),
                            IdCrypto = Convert.ToInt32(data["IdCrypto"]),
                            Stock = Convert.ToDouble(data["Stock"]),
                            PorteFeuilleOwn = _dbContext.Portefeuilles.FirstOrDefault(p => p.IdPortefeuille == Convert.ToInt32(data["IdPortefeuille"]))
                        };
                        _dbContext.Stocks.Add(stock);
                    }
                }
                await _dbContext.SaveChangesAsync();
                Console.WriteLine("Migration des stocks terminée !");
            }
        }

        private async Task MigrateHistoriqueAsync()
        {
            var historiquesRef = _firestore.Collection("historiquesprix");
            var snapshot = await historiquesRef.GetSnapshotAsync();

            if (snapshot.Documents.Any())
            {
                Console.WriteLine("Migration des historiques en cours...");
                foreach (var document in snapshot.Documents)
                {
                    var historiqueDto = document.ConvertTo<HistoriquePrixFirebaseDto>();
                    var historique = _mapper.Map<HistoriquePrix>(historiqueDto);

                    var existingHistorique = await _dbContext.Historiques
                        .FirstOrDefaultAsync(h => h.IdHistorique == historique.IdHistorique);

                    if (existingHistorique == null)
                    {
                        _dbContext.Historiques.Add(historique);
                    }
                    else
                    {
                        existingHistorique.PrixCrypto = historique.PrixCrypto;
                        existingHistorique.DateChange = historique.DateChange;
                    }
                }
                await _dbContext.SaveChangesAsync();
                Console.WriteLine("Migration des historiques terminée !");
            }
        }
    }
}
