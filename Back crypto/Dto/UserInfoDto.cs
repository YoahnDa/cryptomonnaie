namespace Backend_Crypto.Dto
{
    public class UserInfoDto
    {
        public int id { get; set; }
        public string username { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public bool isAdmin { get; set; }
    }
}
