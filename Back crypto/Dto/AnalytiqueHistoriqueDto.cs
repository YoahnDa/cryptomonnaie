using System.ComponentModel.DataAnnotations;

namespace Backend_Crypto.Dto
{
    public class AnalytiqueHistoriqueDto
    {
        public int IdHistorique { get; set; }

        [Required(ErrorMessage = "Prix invalide")]
        public double PrixCrypto { get; set; }

        [Required]
        public DateTime DateChange { get; set; } = DateTime.UtcNow;

        // Données analytiques supplémentaires
        public double PremierQuartile { get; set; }
        public bool isMax { get; set; }
        public bool isMin { get; set; }
        public double Moyenne { get; set; }
        public double EcartType { get; set; }
    }
}
