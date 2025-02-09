using System.ComponentModel.DataAnnotations;

namespace Backend_Crypto.Dto
{
    public class OrdreFormDto
    {
        [Range(1, double.MaxValue, ErrorMessage = "La quantite doit être plus grand que 0.")]
        public double quantite;
        public int idCrypto;
    }
}
