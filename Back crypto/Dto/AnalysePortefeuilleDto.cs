using System.ComponentModel;

namespace Backend_Crypto.Dto
{
    public class AnalysePortefeuilleDto
    {
        public int IdPortefeuille { get; set; }

        [DefaultValue(0)]
        public double Fond { get; set; } = 0;
        public ICollection<StockDto> Stock { get; set; } = new List<StockDto>();
        public ICollection<TransactionDtoAdmin> listeEchange { get; set; }
        public ICollection<TransactionDtoAdmin> listeOperation { get; set; }
    }
}
