using System.Threading.Tasks;
using AssetsMonitor.Models;

namespace AssetsMonitor.Interfaces
{
    public interface IAssetService
    {
        Task<AssetDto> GetAssetQuoteAsync(string symbol);
    }
}