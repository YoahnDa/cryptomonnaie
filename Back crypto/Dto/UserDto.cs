namespace Backend_Crypto.Dto
{
    public class UserDto
    {
        public int id { get; set; }
        public string username { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public PortefeuilleDto? portefeuille { get; set;} = null;
    }
}
