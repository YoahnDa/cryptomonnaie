using Backend_Crypto.Models;
using Google.Cloud.Firestore;
using System.ComponentModel;

namespace Backend_Crypto.Dto
{
    [FirestoreData]
    public class StockFirebaseDto
    {
        [FirestoreProperty]
        public int IdStock { get; set; }
        [FirestoreProperty]
        public PortefeuilleFirebaseDto PorteFeuilleOwn { get; set; } = new PortefeuilleFirebaseDto();
        [FirestoreProperty]
        public int IdCrypto { get; set; }
        [FirestoreProperty]
        [DefaultValue(0)]
        public double Stock { get; set; } = 0;
    }
}
