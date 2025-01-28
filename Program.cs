using AssetsMonitor.Workers;
using AssetsMonitor.Services;
using AssetsMonitor.Interfaces;
using AssetsMonitor.Models;
using AssetsMonitor.Settings;
using System.Net.Http;
using AssetsMonitor.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string symbol;
decimal sellPrice;
decimal buyPrice;

if (args.Length == 3)
{
    symbol = args[0];
    if (!decimal.TryParse(args[1], out sellPrice) || !decimal.TryParse(args[2], out buyPrice))
    {
        Console.WriteLine("Invalid price values.");
        return;
    }
    else
    {
        Console.WriteLine($"Monitoring {symbol} with sell price of {sellPrice} and buy price of {buyPrice}");
    }
}
else
{
    Console.WriteLine("Usage: stock-quote-alert.exe <symbol> <sellPrice> <buyPrice>");
    symbol = "PETR4";
    buyPrice = 36.59M;
    sellPrice = 37.62M;
}

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("AssetsMonitorDb"));

// Load configuration
var smtpSettings = builder.Configuration.GetSection("SmtpSettings").Get<SmtpSettings>();
var alertSettings = builder.Configuration.GetSection("AlertSettings").Get<AlertSettings>();
var apiSettings = builder.Configuration.GetSection("AlphaVantageApiSettings").Get<AlphaVantageApiSettings>();

// Register configuration instances
builder.Services.AddSingleton(smtpSettings);
builder.Services.AddSingleton(alertSettings);
builder.Services.AddSingleton(apiSettings);

// Register services
builder.Services.AddScoped<IAssetsService, AssetsService>();
builder.Services.AddTransient<IEmailSenderService, EmailSenderService>();
builder.Services.AddHttpClient<IAlphaVantageApi, AlphaVantageApi>();


// Register worker
builder.Services.AddHostedService(sp =>
{
    var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
    var scope = scopeFactory.CreateScope();
    return new AssetMonitorWorker(
        scope.ServiceProvider.GetRequiredService<IAssetsService>(),
        sp.GetRequiredService<IEmailSenderService>(),
        symbol,
        sellPrice,
        buyPrice,
        sp.GetRequiredService<ILogger<AssetMonitorWorker>>()
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();