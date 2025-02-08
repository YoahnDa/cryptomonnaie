namespace Backend_Crypto.Models
{
    public class Favoris
    {
        public int IdFavoris { get; set; }
        public int idUser { get; set; }
        public int idCrypto { get; set; }
        public Crypto Cryptos { get; set; } = new Crypto();
    }
}
