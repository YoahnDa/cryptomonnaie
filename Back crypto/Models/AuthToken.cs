using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_Crypto.Models
{
    public class AuthToken
    {
        public int IdToken { get; set; }
        [Column(TypeName = "TEXT")]
        public string Token { get; set; } = string.Empty;
        [Required]
        public bool IsUsed { get; set; } = false;
        public int IdTransaction { get; set; }
        public Transaction Transac { get; set; } = new Transaction();
    }
}
