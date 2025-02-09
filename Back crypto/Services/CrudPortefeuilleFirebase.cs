using Backend_Crypto.Dto;
using Google.Cloud.Firestore;

namespace Backend_Crypto.Services
{
    public class CrudPortefeuilleFirebase
    {
        private readonly FirestoreDb _firestoreDb;

        public CrudPortefeuilleFirebase()
        {
            _firestoreDb = FirestoreDb.Create("testfirebaseproject-8cc5e");
        }

        public async Task CreatePortefeuilleAsync(PortefeuilleFirebaseDto portefeuille)
        {
            var cryptoRef = _firestoreDb.Collection("portefeuilles").Document(portefeuille.IdPortefeuille.ToString());
            await cryptoRef.SetAsync(portefeuille);
        }

        // Mettre à jour une historique
        public async Task UpdatePortefeuilleAsync(PortefeuilleFirebaseDto portefeuille)
        {
            var cryptoRef = _firestoreDb.Collection("portefeuilles").Document(portefeuille.IdPortefeuille.ToString());
            await cryptoRef.SetAsync(portefeuille, SetOptions.MergeAll);
        }

        // Delete historique
        public async Task DeletePortefeuilleAsync(string id)
        {
            var cryptoRef = _firestoreDb.Collection("portefeuilles").Document(id);
            await cryptoRef.DeleteAsync();
        }

        public async Task<List<PortefeuilleFirebaseDto>> GetAllPortefeuilleAsync()
        {
            var histoRef = _firestoreDb.Collection("portefeuilles");
            var snapshot = await histoRef.GetSnapshotAsync();
            var histoList = new List<PortefeuilleFirebaseDto>();

            foreach (var document in snapshot.Documents)
            {
                var crypto = document.ConvertTo<PortefeuilleFirebaseDto>();
                histoList.Add(crypto);
            }

            return histoList;
        }
    }
}
