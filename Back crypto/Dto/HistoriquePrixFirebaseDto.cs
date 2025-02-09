using Google.Cloud.Firestore;
using System.ComponentModel.DataAnnotations;

namespace Backend_Crypto.Dto
{
    [FirestoreData]
    public class HistoriquePrixFirebaseDto
    {
        [FirestoreProperty]
        public int IdHistorique { get; set; }
        [FirestoreProperty]
        [Required(ErrorMessage = "Prix invalide")]
        public double PrixCrypto { get; set; }
        [FirestoreProperty]
        [Required]
        public DateTime DateChange { get; set; } = DateTime.UtcNow;
        [FirestoreProperty]
        public int idCrypto { get; set; }
    }
}
