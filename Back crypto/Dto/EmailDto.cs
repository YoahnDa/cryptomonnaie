using System.ComponentModel.DataAnnotations;

namespace Backend_Crypto.Dto
{
    public class EmailDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "L'email est manquante")]
        [EmailAddress(ErrorMessage = "Email invalide")]
        public string value { get; set; } = string.Empty;
        public bool isVerified { get; set; } = false;
    }
}
