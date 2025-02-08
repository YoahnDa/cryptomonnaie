using Backend_Crypto.Models;
using System.ComponentModel.DataAnnotations;

namespace Backend_Crypto.Dto
{
    public class TransactionDtoAdmin
    {
        public int IdTransaction { get; set; }

        [Required]
        public String Type { get; set; }

        [Required]
        public String State { get; set; } 
        public DateTime DateTransaction { get; set; }
        public int idUser { get; set; }
        public String nom { get; set; }
        public double fond { get; set; }
    }
}
