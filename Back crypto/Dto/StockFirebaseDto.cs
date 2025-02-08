using Backend_Crypto.Models;
using System.ComponentModel;

namespace Backend_Crypto.Dto
{
    public class StockFirebaseDto
    {
        public int IdStock { get; set; }
        public PortefeuilleFirebaseDto PorteFeuilleOwn { get; set; } = new PortefeuilleFirebaseDto();
        public int IdCrypto { get; set; }
        [DefaultValue(0)]
        public double Stock { get; set; } = 0;
    }
}
