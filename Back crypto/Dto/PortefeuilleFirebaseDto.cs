using System.ComponentModel;

namespace Backend_Crypto.Dto
{
    public class PortefeuilleFirebaseDto
    {
        public int IdPortefeuille { get; set; }
        public int IdUser { get; set; }

        [DefaultValue(0)]
        public double Fond { get; set; } = 0;
    }
}
