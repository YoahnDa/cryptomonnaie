using System.ComponentModel.DataAnnotations;

namespace Backend_Crypto.Dto
{
    public class UserInscriptionDto
    {
        [Required(ErrorMessage = "L'username est obligatoire")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le mot de passe est obligatoire")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'email est manquante")]
        [EmailAddress(ErrorMessage = "Email invalide")]
        public string Email { get; set; } = string.Empty;
    }
}
