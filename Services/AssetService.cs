using System.Threading.Tasks;
using AssetsMonitor.Models;
using AssetsMonitor.Interfaces;
using Microsoft.Extensions.Logging;
using AssetsMonitor.Mappers;

namespace AssetsMonitor.Services
{
    public class AssetService : IAssetService
    {
        private readonly IAlphaVantageApi _apiClient;
        private readonly ILogger<AssetService> _logger;

        public AssetService(IAlphaVantageApi apiClient, ILogger<AssetService> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        public async Task<GlobalQuote> GetAssetQuoteAsync(string symbol)
        {
            _logger.LogInformation("Iniciando a consulta de cotação para o ativo {Symbol}", symbol);

            try
            {
                var response = await _apiClient.GetGlobalQuoteAsync(symbol);
                _logger.LogInformation("Resposta recebida da API para o ativo {Symbol}: \n\n{Response}\n\n", symbol, response);

                var assetQuote = GlobalQuoteMapper.MapFromJson(response);
                _logger.LogInformation("Cotação do ativo {Symbol} processada com sucesso", symbol);

                return assetQuote;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar a cotação do ativo {Symbol}", symbol);
                throw;
            }
        }
    }
}

