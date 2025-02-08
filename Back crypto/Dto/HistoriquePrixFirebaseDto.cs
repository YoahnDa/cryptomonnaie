using System.ComponentModel.DataAnnotations;

namespace Backend_Crypto.Dto
{
    public class HistoriquePrixFirebaseDto
    {
        public int IdHistorique { get; set; }

        [Required(ErrorMessage = "Prix invalide")]
        public double PrixCrypto { get; set; }

        [Required]
        public DateTime DateChange { get; set; } = DateTime.UtcNow;
        public int idCrypto { get; set; }
    }
}
