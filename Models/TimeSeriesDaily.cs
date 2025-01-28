using System;
using System.Text.Json.Serialization;

namespace AssetsMonitor.Models
{ 
    public class TimeSeriesDaily
    {
        public MetaData MetaData { get; set; }
        public Dictionary<string, DailyData> TimeSeries { get; set; }
    }

    public class MetaData
    {
        public string Information { get; set; }
        public string Symbol { get; set; }
        public DateTime LastRefreshed { get; set; }
        public string OutputSize { get; set; }
        public string TimeZone { get; set; }
    }

    public class DailyData
    {
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public long Volume { get; set; }
    }
}
