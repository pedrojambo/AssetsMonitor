using System;
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
                Symbol = globalQuoteElement.GetProperty("01. symbol").GetString(),
                Open = decimal.Parse(globalQuoteElement.GetProperty("02. open").GetString()),
                High = decimal.Parse(globalQuoteElement.GetProperty("03. high").GetString()),
                Low = decimal.Parse(globalQuoteElement.GetProperty("04. low").GetString()),
                Price = decimal.Parse(globalQuoteElement.GetProperty("05. price").GetString()),
                Volume = long.Parse(globalQuoteElement.GetProperty("06. volume").GetString()),
                LatestTradingDay = DateTime.Parse(globalQuoteElement.GetProperty("07. latest trading day").GetString()),
                PreviousClose = decimal.Parse(globalQuoteElement.GetProperty("08. previous close").GetString()),
                Change = decimal.Parse(globalQuoteElement.GetProperty("09. change").GetString()),
                ChangePercent = globalQuoteElement.GetProperty("10. change percent").GetString()
            };
        }
    }
}

