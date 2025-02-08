using Backend_Crypto.Data;
using Backend_Crypto.Models;

namespace Backend_Crypto.Interfaces
{
    public interface IHistoriqueRepository
    {
        ICollection<HistoriquePrix> GetHistorique(int idCrypto);
        ICollection<HistoriquePrix> GetHistoriqueReverser(int idCrypto);
        bool CreateHistorique(HistoriquePrix histo);
        bool updateHistorique(HistoriquePrix histo);
        bool removeHistorique(HistoriquePrix histo);
        bool Save();
    }
}
