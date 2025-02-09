using Backend_Crypto.Dto;
using Backend_Crypto.Interfaces;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;

namespace Backend_Crypto.Services
{
    public class CrudCryptoFirebase
    {
        private readonly FirestoreDb _firestoreDb;
        private readonly ICryptoRepository _cryptoRepository;

        public CrudCryptoFirebase(ICryptoRepository cryptoRepository)
        {
            _firestoreDb =  FirestoreDb.Create("testfirebaseproject-8cc5e");
            _cryptoRepository = cryptoRepository;
        }

        // Créer une Crypto
        public async Task CreateCryptoAsync(CryptoFirebaseDto crypto)
        {
            var cryptoRef = _firestoreDb.Collection("cryptos").Document(crypto.IdCrypto.ToString());
            await cryptoRef.SetAsync(new
            {
                crypto.IdCrypto,
                crypto.Nom,
                crypto.Symbole,
                prix = _cryptoRepository.GetPrixCrypto(crypto.IdCrypto)
            });
        }

        // Lire une Crypto
        public async Task<CryptoFirebaseDto> GetCryptoAsync(string id)
        {
            var cryptoRef = _firestoreDb.Collection("cryptos").Document(id);
            var snapshot = await cryptoRef.GetSnapshotAsync();

            if (snapshot.Exists)
            {
                return snapshot.ConvertTo<CryptoFirebaseDto>();
            }
            return null;
        }

        // Mettre à jour une Crypto
        public async Task UpdateCryptoAsync(CryptoFirebaseDto crypto)
        {
            var cryptoRef = _firestoreDb.Collection("cryptos").Document(crypto.IdCrypto.ToString());
            await cryptoRef.SetAsync(new
            {
                crypto.IdCrypto,
                crypto.Nom,
                crypto.Symbole,
                prix = _cryptoRepository.GetPrixCrypto(crypto.IdCrypto)
            }, SetOptions.MergeAll);
        }

        public async Task DeleteCryptoAsync(string id)
        {
            var cryptoRef = _firestoreDb.Collection("cryptos").Document(id);
            await cryptoRef.DeleteAsync();
        }

        public async Task<List<CryptoFirebaseDto>> GetAllCryptosAsync()
        {
            var cryptosRef = _firestoreDb.Collection("cryptos");
            var snapshot = await cryptosRef.GetSnapshotAsync();
            var cryptoList = new List<CryptoFirebaseDto>();

            foreach (var document in snapshot.Documents)
            {
                var crypto = document.ConvertTo<CryptoFirebaseDto>();
                cryptoList.Add(crypto);
            }

            return cryptoList;
        }
    }
}
