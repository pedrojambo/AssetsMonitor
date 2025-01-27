using System.Net.Http;
using System.Threading.Tasks;
using AssetsMonitor.Models;
using AssetsMonitor.Interfaces;
using AssetsMonitor.Settings;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using AssetsMonitor.Mappers;

namespace AssetsMonitor.Services
{
    public class AssetService : IAssetService
    {
        private readonly HttpClient _httpClient;
        private readonly AlphaVantageApiSettings _apiSettings;
        private readonly ILogger<AssetService> _logger;

        public AssetService(HttpClient httpClient, AlphaVantageApiSettings apiSettings, ILogger<AssetService> logger)
        {
            _httpClient = httpClient;
            _apiSettings = apiSettings;
            _logger = logger;
        }

        public async Task<GlobalQuote> GetAssetQuoteAsync(string symbol)
        {
            _logger.LogInformation("Iniciando a consulta de cotação para o ativo {Symbol}", symbol);

            try
            {
                string apiKey = _apiSettings.ApiKey;
                string regionalSufix = _apiSettings.RegionalSufix;
                string function = _apiSettings.Function;
                string baseUrl = _apiSettings.BaseUrl;

                var response = await _httpClient.GetStringAsync($"{baseUrl}?function={function}&symbol={symbol}.{regionalSufix}&apikey={apiKey}");
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