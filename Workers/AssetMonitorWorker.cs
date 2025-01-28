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
        private readonly IAssetsService _assetsService;
        private readonly IEmailSenderService _emailSenderService;
        private readonly string _assetSymbol;
        private readonly decimal _sellPrice;
        private readonly decimal _buyPrice;
        private readonly ILogger<AssetMonitorWorker> _logger;

        public AssetMonitorWorker(IAssetsService assetsService, IEmailSenderService emailSenderService, string assetSymbol, decimal sellPrice, decimal buyPrice, ILogger<AssetMonitorWorker> logger)
        {
            _assetsService = assetsService;
            _emailSenderService = emailSenderService;
            _assetSymbol = assetSymbol;
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
                    var asset = await _assetsService.GetAssetQuoteAsync(_assetSymbol);
                    _logger.LogInformation("Cotacao do ativo {AssetSymbol} recebida: {Price}", _assetSymbol, asset.Price);

                    if (asset.Price >= _sellPrice)
                    {
                        _logger.LogInformation("Preco do ativo {AssetSymbol} atingiu o Preco de venda: {Price} > {sellPrice}", _assetSymbol, asset.Price, _sellPrice);
                        await _emailSenderService.SendAlertEmailAsync($"Alerta de Venda: {_assetSymbol}", _assetSymbol, asset.Price, "Venda");
                    }
                    else if (asset.Price <= _buyPrice)
                    {
                        _logger.LogInformation("Preco do ativo {AssetSymbol} atingiu o Preco de compra: {Price} < {buyPrice}", _assetSymbol, asset.Price, _buyPrice);
                        await _emailSenderService.SendAlertEmailAsync($"Alerta de Compra: {_assetSymbol}", _assetSymbol, asset.Price, "Compra");
                    }
                    else
                    {
                        _logger.LogInformation("Preco do ativo {AssetSymbol} esta dentro da faixa de monitoramento: {sellPrice} - {buyPrice}", _assetSymbol, _sellPrice, _buyPrice);
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