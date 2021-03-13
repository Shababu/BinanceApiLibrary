using Newtonsoft.Json;

namespace BinanceApiLibrary.Deserialization.Trades
{
    public class FilledTrade
    {
        public string Symbol { get; set; }
        public string OrderId { get; set; }
        public double Price { get; set; }
        public double Qty { get; set; }
        public double QuoteQty { get; set; }
        public double Commission { get; set; }
        public string TimeStamp { get; set; }
        public bool IsBuyer { get; set; }
        public bool IsMaker { get; set; }

        public static FilledTrade DeserializeOrder(string json)
        {
            FilledTrade stats = JsonConvert.DeserializeObject<FilledTrade>(json);
            return stats;
        }

        public override string ToString()
        {
            string side;
            string boughtOrSold;
            string spentOrRecived;
            if (IsBuyer)
            {
                side = "Buy";
                boughtOrSold = "Bought";
                spentOrRecived = "Spent";
            }
            else
            {
                side = "Sell";
                boughtOrSold = "Sold";
                spentOrRecived = "Recived";
            }
            return string.Format($"OrderId: {OrderId}, Timestamp: {TimeStamp} {side}, Symbol: {Symbol}, Price: {Price}, Quantuty {boughtOrSold}: {Qty}, Quantuty {spentOrRecived}: {QuoteQty}, Commission: {Commission}");
        }
    }     
}
