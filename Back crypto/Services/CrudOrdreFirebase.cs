using Backend_Crypto.Dto;
using Google.Cloud.Firestore;

namespace Backend_Crypto.Services
{
    public class CrudOrdreFirebase
    {
        private readonly FirestoreDb _firestoreDb;

        public CrudOrdreFirebase()
        {
            _firestoreDb = FirestoreDb.Create("testfirebaseproject-8cc5e");
        }

        public async Task CreateOrdreAsync(OrdreFirebaseDto ordre)
        {
            var cryptoRef = _firestoreDb.Collection("ordres").Document(ordre.IdOrdre.ToString());
            await cryptoRef.SetAsync(ordre);
        }

        // Mettre à jour une historique
        public async Task UpdateOrdreAsync(OrdreFirebaseDto ordre)
        {
            var cryptoRef = _firestoreDb.Collection("ordres").Document(ordre.IdOrdre.ToString());
            await cryptoRef.SetAsync(ordre, SetOptions.MergeAll);
        }

        // Delete historique
        public async Task DeleteOrdreAsync(string id)
        {
            var cryptoRef = _firestoreDb.Collection("ordres").Document(id);
            await cryptoRef.DeleteAsync();
        }

        public async Task<List<OrdreFirebaseDto>> GetAllOrdresAsync()
        {
            var histoRef = _firestoreDb.Collection("ordres");
            var snapshot = await histoRef.GetSnapshotAsync();
            var histoList = new List<OrdreFirebaseDto>();

            foreach (var document in snapshot.Documents)
            {
                var crypto = document.ConvertTo<OrdreFirebaseDto>();
                histoList.Add(crypto);
            }

            return histoList;
        }
    }
}
