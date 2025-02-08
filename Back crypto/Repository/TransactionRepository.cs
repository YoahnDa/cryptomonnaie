using Backend_Crypto.Data;
using Backend_Crypto.Interfaces;
using Backend_Crypto.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend_Crypto.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private DataContext _context;

        public TransactionRepository(DataContext context)
        {
            _context = context;
        }
        public bool ChangeEtat(Transaction transac, Status state)
        {
            transac.State = state;
            return Save();
        }

        public bool CreateTransaction(TypeTransaction type, Portefeuille porte, double fond = 0 , Ordre ordre = null)
        {
            Transaction transac = new Transaction() 
            { 
                Type = type,
                PortefeuilleOwner = porte,
                fond = fond,
                Ordre = ordre
            };
            _context.Add(transac);
            return Save();
        }

        public bool DeleteTransaction(Transaction transac)
        {
            return Remove(transac);
        }

        public Transaction? GetTransaction(int idTransac)
        {
            return _context.Transac .FirstOrDefault(c => c.IdTransaction == idTransac);
        }

        public ICollection<Transaction> GetTransactionPortefeuille(Portefeuille portefeuille)
        {
            var transac = _context.Transac
                          .Include(c => c.PortefeuilleOwner)
                          .Where(c => c.PortefeuilleOwner.IdPortefeuille == portefeuille.IdPortefeuille)
                          .ToList();
            return transac ?? new List<Transaction>();
        }

        public bool Remove(Transaction transac)
        {
            _context.Remove(transac);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool ValidationTransaction(Transaction transac)
        {
            transac.State = Status.Valid;
            _context.Update(transac);
            return Save();
        }

        public ICollection<Transaction> GetTransactionByTypes(List<TypeTransaction> types)
        {
            return _context.Transac
                            .Where(t => types.Contains(t.Type)) // Filtrer les types demandés
                            .ToList();
        }

        public ICollection<Transaction> GetTransactionByTypesPortefeuille(List<TypeTransaction> types, int idPortefeuille)
        {
            return _context.Transac
                            .Where(t => t.IdPortefeuille == idPortefeuille && types.Contains(t.Type)) // Filtrer les types demandés
                            .ToList();
        }
    }
}
