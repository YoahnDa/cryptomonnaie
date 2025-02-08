using Backend_Crypto.Models;
using Transac = Backend_Crypto.Models.Transaction;

namespace Backend_Crypto.Interfaces
{
    public interface ITransactionRepository
    {
        bool ChangeEtat(Transac transac , Status state);
        bool CreateTransaction(TypeTransaction type , Portefeuille porte , double fond = 0, Ordre ordre = null);
        bool DeleteTransaction(Transac transac);
        Transac GetTransaction(int idTransac);
        ICollection<Transac> GetTransactionPortefeuille(Portefeuille portefeuille); 
        bool ValidationTransaction(Transac transac);
        bool Remove(Transac transac);
        ICollection<Transac> GetTransactionByTypes(List<TypeTransaction> types);
        ICollection<Transac> GetTransactionByTypesPortefeuille(List<TypeTransaction> types,int idPortefeuille);
        bool Save();
    }
}
