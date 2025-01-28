using System.Net.Http;
using System.Threading.Tasks;
using AssetsMonitor.Settings;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using AssetsMonitor.Models;
using AssetsMonitor.Interfaces;

namespace AssetsMonitor.Services
{
    public class AlphaVantageApi : IAlphaVantageApi
    {
        private readonly HttpClient _httpClient;
        private readonly AlphaVantageApiSettings _apiSettings;
        private readonly ILogger<AlphaVantageApi> _logger;

        public AlphaVantageApi(HttpClient httpClient, AlphaVantageApiSettings apiSettings, ILogger<AlphaVantageApi> logger)
        {
            _httpClient = httpClient;
            _apiSettings = apiSettings;
            _logger = logger;
        }

        public async Task<string> GetGlobalQuoteAsync(string symbol)
        {
            _logger.LogInformation("Iniciando a consulta à API Alpha Vantage para a função GLOBAL_QUOTE e o ativo {Symbol}", symbol);

            try
            {
                var response = await _httpClient.GetStringAsync($"{_apiSettings.BaseUrl}?function=GLOBAL_QUOTE&symbol={symbol}.{_apiSettings.RegionalSufix}&apikey={_apiSettings.ApiKey}");
                _logger.LogInformation("Resposta recebida da API para a função GLOBAL_QUOTE e o ativo {Symbol}: \n\n{Response}\n\n", symbol, response);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar a API Alpha Vantage para a função GLOBAL_QUOTE e o ativo {Symbol}", symbol);
                throw;
            }
        }

        public async Task<string> GetTimeSeriesDailyAsync(string symbol)
        {
            _logger.LogInformation("Iniciando a consulta à API Alpha Vantage para a função TIME_SERIES_DAILY e o ativo {Symbol}", symbol);

            try
            {
                var response = await _httpClient.GetStringAsync($"{_apiSettings.BaseUrl}?function=TIME_SERIES_DAILY&symbol={symbol}.{_apiSettings.RegionalSufix}&apikey={_apiSettings.ApiKey}");
                _logger.LogInformation("Resposta recebida da API para a função TIME_SERIES_DAILY e o ativo {Symbol}: \n\n{Response}\n\n", symbol, response);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar a API Alpha Vantage para a função TIME_SERIES_DAILY e o ativo {Symbol}", symbol);
                throw;
            }
        }
    }
}
