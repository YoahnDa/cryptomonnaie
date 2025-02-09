using Backend_Crypto.Dto;
using Google.Cloud.Firestore;

namespace Backend_Crypto.Services
{
    public class CrudFavorisFirebase
    {
        private readonly FirestoreDb _firestoreDb;

        public CrudFavorisFirebase()
        {
            _firestoreDb = FirestoreDb.Create("testfirebaseproject-8cc5e");
        }

        // Créer une historique
        public async Task CreateFavsAsync(FavorisDto favoris)
        {
            var cryptoRef = _firestoreDb.Collection("favoris").Document(favoris.IdFavoris.ToString());
            await cryptoRef.SetAsync(favoris);
        }

        // Mettre à jour une historique
        public async Task UpdateFavsAsync(FavorisDto favoris)
        {
            var cryptoRef = _firestoreDb.Collection("favoris").Document(favoris.IdFavoris.ToString());
            await cryptoRef.SetAsync(favoris, SetOptions.MergeAll);
        }

        // Delete historique
        public async Task DeleteFavsAsync(string id)
        {
            var cryptoRef = _firestoreDb.Collection("favoris").Document(id);
            await cryptoRef.DeleteAsync();
        }

        public async Task<List<FavorisDto>> GetAllFavsAsync()
        {
            var histoRef = _firestoreDb.Collection("favoris");
            var snapshot = await histoRef.GetSnapshotAsync();
            var histoList = new List<FavorisDto>();

            foreach (var document in snapshot.Documents)
            {
                var crypto = document.ConvertTo<FavorisDto>();
                histoList.Add(crypto);
            }

            return histoList;
        }
    }
}
