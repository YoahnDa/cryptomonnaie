using System.ComponentModel.DataAnnotations;

namespace Backend_Crypto.Dto
{
    public class UserConnectDto
    {
        [Required(ErrorMessage = "Username obligatoire.")]
        public string username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mot de passe obligatoire")]
        public string password { get; set; } = string.Empty;
    }
}
