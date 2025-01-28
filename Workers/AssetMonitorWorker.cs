using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AssetsMonitor.Interfaces;
using AssetsMonitor.Models;
using AssetsMonitor.Settings;

namespace AssetsMonitor.Workers
{
    public class AssetMonitorWorker : BackgroundService
    {
        private readonly IAssetService _assetService;
        private readonly IEmailSenderService _emailSenderService;
        private readonly string _assetSymbol;
        private readonly AlertSettings _alertSettings;
        private readonly decimal _sellPrice;
        private readonly decimal _buyPrice;
        private readonly ILogger<AssetMonitorWorker> _logger;

        public AssetMonitorWorker(IAssetService assetService, IEmailSenderService emailSenderService, AlertSettings alertSettings, string assetSymbol, decimal sellPrice, decimal buyPrice, ILogger<AssetMonitorWorker> logger)
        {
            _assetService = assetService;
            _emailSenderService = emailSenderService;
            _assetSymbol = assetSymbol;
            _alertSettings = alertSettings;
            _sellPrice = sellPrice;
            _buyPrice = buyPrice;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Iniciando o monitoramento do ativo {AssetSymbol}", _assetSymbol);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var asset = await _assetService.GetAssetQuoteAsync(_assetSymbol);
                    _logger.LogInformation("Cotação do ativo {AssetSymbol} recebida: {Price}", _assetSymbol, asset.Price);

                    if (asset.Price >= _sellPrice)
                    {
                        _logger.LogInformation("Preço do ativo {AssetSymbol} atingiu o preço de venda: {Price}", _assetSymbol, asset.Price);
                        await _emailSenderService.SendEmailAsync(_alertSettings.RecipientEmail, "Sell Alert", $"The price of {asset.Symbol} is now {asset.Price}. Consider selling.");
                    }
                    else if (asset.Price <= _buyPrice)
                    {
                        _logger.LogInformation("Preço do ativo {AssetSymbol} atingiu o preço de compra: {Price}", _assetSymbol, asset.Price);
                        await _emailSenderService.SendEmailAsync(_alertSettings.RecipientEmail, "Buy Alert", $"The price of {asset.Symbol} is now {asset.Price}. Consider buying.");
                    }
                    else
                    {
                        _logger.LogInformation("Preço do ativo {AssetSymbol} está dentro da faixa de monitoramento: {Price}", _assetSymbol, asset.Price);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao monitorar o ativo {AssetSymbol}", _assetSymbol);
                }

                await Task.Delay(60000, stoppingToken); 
            }

            _logger.LogInformation("Monitoramento do ativo {AssetSymbol} encerrado", _assetSymbol);
        }
    }
}