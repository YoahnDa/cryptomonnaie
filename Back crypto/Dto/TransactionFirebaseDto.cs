using Backend_Crypto.Models;
using System.ComponentModel.DataAnnotations;

namespace Backend_Crypto.Dto
{
    public class TransactionFirebaseDto
    {
        public int IdTransaction { get; set; }

        [Required]
        public TypeTransaction Type { get; set; }

        [Required]
        public Status State { get; set; } = Status.Waiting;
        public DateTime DateTransaction { get; set; } = DateTime.UtcNow;
        public int IdPortefeuille { get; set; }
        public double fond { get; set; } = 0;
        public PortefeuilleDto PortefeuilleOwner { get; set; } = new PortefeuilleDto();
    }
}
