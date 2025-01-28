using System;
using System.Text.Json.Serialization;

namespace AssetsMonitor.Models
{
    public class GlobalQuote
    {
        public int Id { get; set; }
        public string Symbol { get; set; }

        public decimal Open { get; set; }

        public decimal High { get; set; }

        public decimal Low { get; set; }

        public decimal Price { get; set; }

        public long Volume { get; set; }

        public DateTime LatestTradingDay { get; set; }

        public decimal PreviousClose { get; set; }

        public decimal Change { get; set; }

        public string ChangePercent { get; set; }

        public override string ToString()
        {
            return $"\n\nSymbol: {Symbol}, \nOpen: {Open}, \nHigh: {High}, \nLow: {Low}, \nPrice: {Price}, \nVolume: {Volume}, \nLatestTradingDay: {LatestTradingDay}, \nPreviousClose: {PreviousClose}, \nChange: {Change}, \nChangePercent: {ChangePercent}\n\n";
        }

    }

}

