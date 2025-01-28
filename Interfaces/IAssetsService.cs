using System.Threading.Tasks;
using AssetsMonitor.Models;

namespace AssetsMonitor.Interfaces
{
    public interface IAssetsService
    {
        Task<GlobalQuote> GetAssetQuoteAsync(string symbol);
        Task<List<GlobalQuote>> GetHistoryAsync(string symbol);
    }
}