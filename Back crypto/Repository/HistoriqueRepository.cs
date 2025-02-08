using Backend_Crypto.Data;
using Backend_Crypto.Interfaces;
using Backend_Crypto.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend_Crypto.Repository
{
    public class HistoriqueRepository : IHistoriqueRepository
    {
        private readonly DataContext _context;

        public HistoriqueRepository(DataContext context)
        {
            _context = context;
        }

        public bool CreateHistorique(HistoriquePrix histo)
        {
            _context.Add(histo);
            return Save();
        }

        public ICollection<HistoriquePrix> GetHistorique(int idCrypto)
        {
            var histo = _context.Historiques
                          .Where(c => c.idCrypto == idCrypto)
                          .OrderByDescending(h => h.DateChange)
                          .ToList();
            return histo ?? new List<HistoriquePrix>();
        }

        public ICollection<HistoriquePrix> GetHistoriqueReverser(int idCrypto)
        {
            var histo = _context.Historiques
                          .Where(c => c.idCrypto == idCrypto)
                          .OrderBy(h => h.DateChange)
                          .ToList();
            return histo ?? new List<HistoriquePrix>();
        }

        public bool removeHistorique(HistoriquePrix histo)
        {
            _context.Remove(histo);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool updateHistorique(HistoriquePrix histo)
        {
            _context.Update(histo);
            return Save();
        }
    }
}
