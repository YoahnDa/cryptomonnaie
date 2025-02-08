using System.ComponentModel.DataAnnotations;

namespace Backend_Crypto.Dto
{
    public class TransactionFormDto
    {
        [Range(1, double.MaxValue, ErrorMessage = "La quantite doit être plus grand que 0.")]
        public double quantite { get; set; }
        [Required(ErrorMessage ="Le crypto est obligatoire.")]
        public int idCrypto { get; set; }
    }
}
