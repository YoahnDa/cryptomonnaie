using Backend_Crypto.Dto;
using Google.Cloud.Firestore;

namespace Backend_Crypto.Services
{
    public class CrudHistoriquePrixFirebase
    {
        private readonly FirestoreDb _firestoreDb;

        public CrudHistoriquePrixFirebase()
        {
            _firestoreDb = FirestoreDb.Create("testfirebaseproject-8cc5e");
        }

        // Créer une historique
        public async Task CreateHistoriqueAsync(HistoriquePrixFirebaseDto histo)
        {
            var cryptoRef = _firestoreDb.Collection("historiquesprix").Document(histo.IdHistorique.ToString());
            await cryptoRef.SetAsync(histo);
        }

        // Mettre à jour une historique
        public async Task UpdateHistoAsync(HistoriquePrixFirebaseDto histo)
        {
            var cryptoRef = _firestoreDb.Collection("historiqueprix").Document(histo.IdHistorique.ToString());
            await cryptoRef.SetAsync(histo, SetOptions.MergeAll);
        }

        // Delete historique
        public async Task DeleteHistoAsync(string id)
        {
            var cryptoRef = _firestoreDb.Collection("historiqueprix").Document(id);
            await cryptoRef.DeleteAsync();
        }

        public async Task<List<HistoriquePrixFirebaseDto>> GetAllHistosAsync()
        {
            var histoRef = _firestoreDb.Collection("historiqueprix");
            var snapshot = await histoRef.GetSnapshotAsync();
            var histoList = new List<HistoriquePrixFirebaseDto>();

            foreach (var document in snapshot.Documents)
            {
                var crypto = document.ConvertTo<HistoriquePrixFirebaseDto>();
                histoList.Add(crypto);
            }

            return histoList;
        }
    }
}
