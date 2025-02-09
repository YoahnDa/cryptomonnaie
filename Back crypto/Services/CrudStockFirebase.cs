using Backend_Crypto.Dto;
using Google.Cloud.Firestore;

namespace Backend_Crypto.Services
{
    public class CrudStockFirebase
    {
        private readonly FirestoreDb _firestoreDb;

        public CrudStockFirebase()
        {
            _firestoreDb = FirestoreDb.Create("testfirebaseproject-8cc5e");
        }

        public async Task CreateStockAsync(StockFirebaseDto stock)
        {
            var cryptoRef = _firestoreDb.Collection("stockPortefeuille").Document(stock.IdStock.ToString());
            await cryptoRef.SetAsync(new
            {
                stock.IdStock,
                stock.IdCrypto,
                stock.Stock,
                stock.PorteFeuilleOwn.IdPortefeuille
            });
        }

        // Mettre à jour une historique
        public async Task UpdateStockAsync(StockFirebaseDto stockDto)
        {
            var cryptoRef = _firestoreDb.Collection("stockPortefeuille").Document(stockDto.IdStock.ToString());
            await cryptoRef.SetAsync(new
            {
                stockDto.IdStock,
                stockDto.IdCrypto,
                stockDto.Stock,
                stockDto.PorteFeuilleOwn.IdPortefeuille
            }, SetOptions.MergeAll);
        }

        // Delete historique
        public async Task DeleteStockAsync(string id)
        {
            var cryptoRef = _firestoreDb.Collection("stockPortefeuille").Document(id);
            await cryptoRef.DeleteAsync();
        }
    }
}
