using Backend_Crypto.Models;
using System.ComponentModel.DataAnnotations;

namespace Backend_Crypto.Dto
{
    public class TransactionDto
    {
        public int IdTransaction { get; set; }

        [Required]
        public TypeTransaction Type { get; set; }

        [Required]
        public Status State { get; set; } = Status.Waiting;
        public DateTime DateTransaction { get; set; } = DateTime.UtcNow;
    }
}
