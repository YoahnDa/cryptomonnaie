namespace Backend_Crypto.Services
{
    public class BrownianMotionPriceSimulator
    {
        private static Random _random = new Random();
        public  decimal _initialPrice { get; private set; }
        public  decimal _drift { get; private set; }
        public  decimal _volatility { get; private set; }
        public decimal _liquidity { get; private set; } // Liquidité de la crypto, choisie au hasard
        public decimal _demand { get; private set; } // Demande de la crypto, choisie au hasard

        public BrownianMotionPriceSimulator(decimal initialPrice) 
        {
            _initialPrice = initialPrice;
            _volatility = (decimal)_random.NextDouble() * (0.1m - 0.02m) + 0.02m; // Entre 2% et 10% de volatilité
            _drift = (decimal)_random.NextDouble() * (0.0005m - 0.00005m) + 0.00005m; // Dérive entre 0.005% et 0.05% par itération
            _liquidity = (decimal)_random.NextDouble() * (0.5m - 0.05m) + 0.05m; // Liquidité entre 5% et 50%
            _demand = (decimal)_random.NextDouble() * (1.5m - 0.5m) + 0.5m; // Demande entre 50% et 150%
        }

        public decimal SimulatePrice(decimal currentPrice)
        {
            // Générer un bruit aléatoire basé sur une distribution normale (mouvement aléatoire)
            decimal randomNoise = (decimal)(_random.NextDouble() * 2 - 1); // Bruit entre -1 et 1
            decimal change = _drift + _volatility * randomNoise;

            // Calcul du nouveau prix en appliquant le changement calculé
            decimal newPrice = _initialPrice * (1 + change);

            // Mise à jour du prix et retour de la nouvelle valeur
            _initialPrice = newPrice > 0 ? newPrice : _initialPrice; // Assurez-vous que le prix ne devienne jamais négatif
            return _initialPrice;
        }
    }
}
