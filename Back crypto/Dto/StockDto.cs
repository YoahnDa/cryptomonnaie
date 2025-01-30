using Backend_Crypto.Models;
using System.ComponentModel;

namespace Backend_Crypto.Dto
{
    public class StockDto
    {
        public int IdCrypto { get; set; }
        public CryptoDto CryptoIn { get; set; } = new CryptoDto();

        [DefaultValue(0)]
        public double Stock { get; set; } = 0;
    }
}
