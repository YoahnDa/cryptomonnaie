using Google.Cloud.Firestore;
using System.ComponentModel;

namespace Backend_Crypto.Dto
{
    [FirestoreData]
    public class PortefeuilleFirebaseDto
    {
        [FirestoreProperty]
        public int IdPortefeuille { get; set; }
        [FirestoreProperty]
        public int IdUser { get; set; }
        [FirestoreProperty]
        [DefaultValue(0)]
        public double Fond { get; set; } = 0;
    }
}
