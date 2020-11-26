using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace BinanceApiLibrary.Deserialization.Trades
{
    public class AssetStats
    {
        public string PriceChange { get; set; }
        public string PriceChangePercent { get; set; }
        public string WeightedAvgPrice { get; set; }
        public string LastPrice { get; set; }
        public string LastQty { get; set; }
        public string OpenPrice { get; set; }
        public string HighPrice { get; set; }
        public string LowPrice { get; set; }

        public static AssetStats DeserializeAssetStats(string json)
        {
            AssetStats stats = JsonConvert.DeserializeObject<AssetStats>(json);
            return stats;
        }

        public override string ToString()
        {
            return string.Format($"PriceChange: {PriceChange}, PriceChangePercent: {PriceChangePercent}, WeightedAvgPrice: {WeightedAvgPrice}, " +
                $"LastPrice: {LastPrice}, LastQty: {LastQty}, OpenPrice: {OpenPrice}, HighPrice: {HighPrice}, LowPrice: {LowPrice}");
        }
    }
}
