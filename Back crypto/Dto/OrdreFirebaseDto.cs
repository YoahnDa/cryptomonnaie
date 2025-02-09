using Backend_Crypto.Models;
using Google.Cloud.Firestore;
using System.ComponentModel.DataAnnotations;

namespace Backend_Crypto.Dto
{
    [FirestoreData]
    public class OrdreFirebaseDto
    {
        [FirestoreProperty]
        public int IdOrdre { get; set; }
        [FirestoreProperty]
        [Range(1, double.MaxValue, ErrorMessage = "Le prix doit être plus grand que 0.")]
        //[Required(ErrorMessage = "Le prix unitaire est nécéssaire")]
        public double PrixUnitaire { get; set; }
        [FirestoreProperty]
        [Required(ErrorMessage = "Quantité incorrecte")]
        public double AmountCrypto { get; set; }
        [FirestoreProperty]
        public int? IdTransaction { get; set; }
        [FirestoreProperty]
        public int IdCrypto { get; set; }
    }
}
