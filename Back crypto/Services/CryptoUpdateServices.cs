using AutoMapper;
using Backend_Crypto.Data;
using Backend_Crypto.Dto;
using Backend_Crypto.Interfaces;
using Backend_Crypto.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Backend_Crypto.Services
{
    public class CryptoUpdateServices : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<CryptoUpdateServices> _logger;

        public CryptoUpdateServices(
            IServiceScopeFactory serviceScopeFactory,
            ILogger<CryptoUpdateServices> logger)
            {
                _serviceScopeFactory = serviceScopeFactory;
                _logger = logger;
            }

        // Logique de mise à jour des prix, cette méthode sera appelée en arrière-plan
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await ExecuteUpdatePrices();
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken); // Attendre 10 secondes avant de relancer
            }
        }

        private async Task ExecuteUpdatePrices()
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var cryptoRepository = scope.ServiceProvider.GetRequiredService<ICryptoRepository>();
                    var historiqueRepository = scope.ServiceProvider.GetRequiredService<IHistoriqueRepository>();
                    var histoFirebase = scope.ServiceProvider.GetRequiredService<CrudHistoriquePrixFirebase>();
                    var cryptoFirebase = scope.ServiceProvider.GetRequiredService<CrudCryptoFirebase>();
                    var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
                    var context = scope.ServiceProvider.GetRequiredService<DataContext>();

                    var cryptos = await cryptoRepository.GetAsynCrypto();
                    if (cryptos.Count == 0)
                    {
                        _logger.LogInformation("Aucune crypto à mettre à jour.");
                        return;
                    }

                    foreach (var crypto in cryptos)
                    {
                        decimal initialPrice = (decimal)cryptoRepository.GetFirstPrixCrypto(crypto.IdCrypto);
                        decimal currentPrice = (decimal)cryptoRepository.GetPrixCrypto(crypto.IdCrypto);
                        BrownianMotionPriceSimulator simulator = new BrownianMotionPriceSimulator(initialPrice);
                        decimal newPrice = simulator.SimulatePrice(currentPrice);

                        HistoriquePrix historiquePrix = new HistoriquePrix()
                        {
                            CryptoChange = crypto,
                            PrixCrypto = (double)newPrice
                        };
                        historiqueRepository.CreateHistorique(historiquePrix);
                        histoFirebase.CreateHistoriqueAsync(mapper.Map<HistoriquePrixFirebaseDto>(historiquePrix));
                        cryptoFirebase.UpdateCryptoAsync(mapper.Map<CryptoFirebaseDto>(historiquePrix.CryptoChange));
                    }

                    _logger.LogInformation($"Prix des cryptos mis à jour à {DateTime.Now}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erreur lors de la mise à jour des prix: {ex.Message}");
            }
        }

    }
}
