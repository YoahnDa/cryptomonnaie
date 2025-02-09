using System.ComponentModel.DataAnnotations;

namespace Backend_Crypto.Dto
{
    public class PinDto
    {
        [Required(ErrorMessage ="Pin obligatoire.")]
        public int pin { get; set; }
    }
}
