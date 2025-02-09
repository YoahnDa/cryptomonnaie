using Backend_Crypto.Dto;
using Google.Cloud.Firestore;

namespace Backend_Crypto.Services
{
    public class CrudTransactionFirebase
    {
        private readonly FirestoreDb _firestoreDb;
        public CrudTransactionFirebase()
        {
            _firestoreDb = FirestoreDb.Create("testfirebaseproject-8cc5e");
        }

        // Créer une historique
        public async Task CreateTransacAsync(TransactionFirebaseDto transactionDto)
        {
            var cryptoRef = _firestoreDb.Collection("transactions").Document(transactionDto.IdTransaction.ToString());
            await cryptoRef.SetAsync(new
            {
                transactionDto.IdTransaction,
                transactionDto.fond,
                transactionDto.DateTransaction,
                transactionDto.IdPortefeuille,
                State = transactionDto.State.ToString(),
                Type = transactionDto.Type.ToString()
            });
        }

        // Mettre à jour une historique
        public async Task UpdateTransacAsync(TransactionFirebaseDto transactionDto)
        {
            var cryptoRef = _firestoreDb.Collection("transactions").Document(transactionDto.IdTransaction.ToString());
            await cryptoRef.SetAsync(new
            {
                transactionDto.IdTransaction,
                transactionDto.fond,
                transactionDto.DateTransaction,
                transactionDto.IdPortefeuille,
                State = transactionDto.State.ToString(),
                Type = transactionDto.Type.ToString()
            }, SetOptions.MergeAll);
        }

        // Delete historique
        public async Task DeleteTransacAsync(string id)
        {
            var cryptoRef = _firestoreDb.Collection("transactions").Document(id);
            await cryptoRef.DeleteAsync();
        }
    }
}
