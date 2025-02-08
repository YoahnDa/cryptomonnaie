using System.ComponentModel.DataAnnotations;

namespace Backend_Crypto.Dto
{
    public class ExchangeFondDto
    {
        [Range(1, double.MaxValue, ErrorMessage = "Le prix doit être plus grand que 0.")]
        public double fond { get; set; }
    }
}
