using System.Threading.Tasks;

namespace AssetsMonitor.Interfaces
{
    public interface IAlphaVantageApi
    {
        Task<string> GetGlobalQuoteAsync(string symbol);
        Task<string> GetTimeSeriesDailyAsync(string symbol);
    }
}