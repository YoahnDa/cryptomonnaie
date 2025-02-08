using Backend_Crypto.Dto;
using Backend_Crypto.Interfaces;

namespace Backend_Crypto.Services
{
    public class AnalytiqueCryptoService
    {
        private readonly IHistoriqueRepository _historiqueRepository;
        private readonly ICryptoRepository _cryptoRepository;

        public AnalytiqueCryptoService(IHistoriqueRepository historiqueRepository, ICryptoRepository cryptoRepository)
        {
            _historiqueRepository = historiqueRepository;
            _cryptoRepository = cryptoRepository;
        }

        public List<AnalytiqueHistoriqueDto> getPrixCrypto(int idCrypto)
        {
            var historique = _historiqueRepository.GetHistorique(idCrypto);
            var prixList = historique.Select(h => h.PrixCrypto).ToList();
            var historiqueList = historique.ToList();

            // Calcul des valeurs statiques avant la boucle
            double max = prixList.Max();
            double min = prixList.Min();
            int idMax = prixList.IndexOf(max);
            int idMin = prixList.IndexOf(min);

            // Initialisation des variables pour la moyenne et l'écart-type
            double somme = 0;
            double sommeCarrée = 0;
            int totalElements = historique.Count;

            List<AnalytiqueHistoriqueDto> result = new List<AnalytiqueHistoriqueDto>();

            // Boucle pour traiter chaque historique
            for (int i = 0; i < totalElements; i++)
            {
                var historiqueItem = historiqueList[i];
                double prix = historiqueItem.PrixCrypto;

                // Calcul de la somme et de la somme carrée pour l'écart-type
                somme += prix;
                sommeCarrée += Math.Pow(prix - somme / (i + 1), 2);

                // Calcul de la moyenne
                double moyenne = somme / (i + 1);

                // Calcul de l'écart-type
                double ecartType = Math.Sqrt(sommeCarrée / (i + 1));

                // Calcul du premier quartile sur les éléments précédents et l'élément actuel
                var prixSubset = prixList.Take(i + 1).OrderBy(x => x).ToList(); // On prend tous les éléments jusqu'à l'index i inclus
                double premierQuartile = CalculerPremierQuartile(prixSubset);

                // Création du DTO avec les valeurs calculées
                var dto = new AnalytiqueHistoriqueDto
                {
                    IdHistorique = historiqueItem.IdHistorique,
                    PrixCrypto = prix,
                    DateChange = historiqueItem.DateChange,
                    Moyenne = moyenne,
                    EcartType = ecartType,
                    PremierQuartile = premierQuartile,
                    isMax = (i == idMax),  // Marque l'élément comme étant le max si l'indice correspond
                    isMin = (i == idMin) // Marque l'élément comme étant le min
                };

                result.Add(dto);
            }

            return result;
        }

        private double CalculerPremierQuartile(List<double> prixSubset)
        {
            int n = prixSubset.Count;
            if (n == 0) return 0; // Sécurité si la liste est vide

            var sortedPrices = prixSubset.OrderBy(p => p).ToList();

            if (n == 1) return sortedPrices[0]; // Si un seul élément, on le retourne

            // Calculer la position du premier quartile
            double posQ1 = (n + 1) / 4.0;
            int posQ1Int = (int)Math.Floor(posQ1); // Utiliser Floor pour éviter d'avoir 0
            double fraction = posQ1 - posQ1Int;

            // Sécurité : s'assurer que posQ1Int est dans les bornes
            if (posQ1Int <= 0) posQ1Int = 1;
            if (posQ1Int >= n) posQ1Int = n - 1;

            double lowerValue = sortedPrices[posQ1Int - 1];
            double upperValue = sortedPrices[posQ1Int];

            // Interpolation entre les deux valeurs
            return lowerValue + fraction * (upperValue - lowerValue);
        }

        public List<CryptoDtoAnalytique> getCrypto()
        {
            var listCrypto = new List<CryptoDtoAnalytique>();
            var crypto = _cryptoRepository.GetCrypto();
            foreach(var cryptoItem in crypto)
            {
                listCrypto.Add(new CryptoDtoAnalytique()
                {
                    IdCrypto = cryptoItem.IdCrypto,
                    Nom = cryptoItem.Nom,
                    Symbole = cryptoItem.Symbole,
                    prix = _cryptoRepository.GetPrixCrypto(cryptoItem.IdCrypto)
                });
            }
            return listCrypto;
        }

    }
}
