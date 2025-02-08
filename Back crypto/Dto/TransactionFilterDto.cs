using Backend_Crypto.Models;

namespace Backend_Crypto.Dto
{
    public class TransactionFilterDto
    {
        public List<int>? IdCryptos { get; set; } 
        public List<int>? IdUtilisateurs { get; set; }
        public List<TypeTransaction> Types { get; set; }   
    }
}
