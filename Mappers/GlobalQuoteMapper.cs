using System;
using System.Globalization;
using System.Text.Json;
using AssetsMonitor.Models;

namespace AssetsMonitor.Mappers
{
    public static class GlobalQuoteMapper
    {
        public static GlobalQuote MapFromJson(string json)
        {
            var jsonDocument = JsonDocument.Parse(json);
            var globalQuoteElement = jsonDocument.RootElement.GetProperty("Global Quote");

            return new GlobalQuote
            {
                Symbol = globalQuoteElement.GetProperty("01. symbol").GetString() ?? string.Empty,
                Open = decimal.Parse(globalQuoteElement.GetProperty("02. open").GetString() ?? "0", CultureInfo.InvariantCulture),
                High = decimal.Parse(globalQuoteElement.GetProperty("03. high").GetString() ?? "0", CultureInfo.InvariantCulture),
                Low = decimal.Parse(globalQuoteElement.GetProperty("04. low").GetString() ?? "0", CultureInfo.InvariantCulture),
                Price = decimal.Parse(globalQuoteElement.GetProperty("05. price").GetString() ?? "0", CultureInfo.InvariantCulture),
                Volume = long.Parse(globalQuoteElement.GetProperty("06. volume").GetString() ?? "0"),
                LatestTradingDay = DateTime.Parse(globalQuoteElement.GetProperty("07. latest trading day").GetString() ?? DateTime.MinValue.ToString()),
                PreviousClose = decimal.Parse(globalQuoteElement.GetProperty("08. previous close").GetString() ?? "0", CultureInfo.InvariantCulture),
                Change = decimal.Parse(globalQuoteElement.GetProperty("09. change").GetString() ?? "0", CultureInfo.InvariantCulture),
                ChangePercent = globalQuoteElement.GetProperty("10. change percent").GetString() ?? string.Empty
            };
        }
    }
}
