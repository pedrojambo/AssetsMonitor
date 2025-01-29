using System.Threading.Tasks;
using AssetsMonitor.Models;
using AssetsMonitor.Interfaces;
using Microsoft.Extensions.Logging;
using AssetsMonitor.Mappers;
using AssetsMonitor.Data;
using AssetsMonitor.Settings;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using AssetsMonitor.Exceptions;

namespace AssetsMonitor.Services
{
    public class AssetsService : IAssetsService
    {
        private readonly IAlphaVantageApi _apiClient;
        private readonly ILogger<AssetsService> _logger;
        private readonly ApplicationDbContext _context;
        private readonly AlphaVantageApiSettings _apiSettings;

        public AssetsService(IAlphaVantageApi apiClient, AlphaVantageApiSettings settings, ILogger<AssetsService> logger, ApplicationDbContext context)
        {
            _apiClient = apiClient;
            _logger = logger;
            _context = context;
            _apiSettings = settings;
        }

        public async Task<GlobalQuote> GetAssetQuoteAsync(string symbol)
        {
            _logger.LogInformation("Iniciando a consulta de cotacao para o ativo {Symbol}", symbol);

            try
            {
                var response = await _apiClient.GetGlobalQuoteAsync(symbol);

                _logger.LogInformation("Resposta recebida da API para o ativo {Symbol}: \n\n{Response}\n\n", symbol, response);

                var assetQuote = GlobalQuoteMapper.MapFromJson(response);
                _logger.LogInformation("Cotacao do ativo {Symbol} processada com sucesso: {AssetQuote}", symbol, assetQuote.ToString());

                _logger.LogInformation("Salvando cotacao no banco de dados local");
                _context.GlobalQuotes.Add(assetQuote);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Cotacao foi salva com sucesso");


                return assetQuote;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar e salvar cotacao do ativo {Symbol}", symbol);
                throw new ApiException("Erro ao consultar e salvar cotacao do ativo", ex);
            }
        }

        public async Task<List<GlobalQuote>> GetHistoryAsync(string code)
        {
            _logger.LogInformation("Iniciando a consulta do historico de cotacoes para o ativo {Symbol}", code);

            string symbol = code + $".{_apiSettings.RegionalSufix}";

            try
            {
                var history = await _context.GlobalQuotes
                    .Where(gq => gq.Symbol == symbol)
                    .OrderByDescending(gq => gq.LatestTradingDay)
                    .ToListAsync();

                _logger.LogInformation("Historico de cotacoes para o ativo {Symbol} recuperado com sucesso", symbol);
                return history;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar o historico de cotacoes do ativo {Symbol}", symbol);
                throw new DatabaseException("Erro ao consultar o historico de cotacoes do ativo", ex);
            }
        }

    }
}

