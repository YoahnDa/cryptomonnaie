using Google.Cloud.Firestore;
using System.ComponentModel.DataAnnotations;

namespace Backend_Crypto.Dto
{
    [FirestoreData]
    public class CryptoFirebaseDto
    {
        [FirestoreProperty]
        public int IdCrypto { get; set; }
        [FirestoreProperty]
        [Required(ErrorMessage = "Le symbole est obligatoire")]
        public string Symbole { get; set; } = string.Empty;
        [FirestoreProperty]
        [Required(ErrorMessage = "Le nom est obligatoire")]
        public string Nom { get; set; } = string.Empty;

    }
}
