namespace BinanceApiLibrary.Trading
{
    public class SpotPosition
    {
        public string Side { get; set; }
        public string OrderId { get; set; }
        public string PreviousOrderId { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
        public decimal Distance { get; set; }
        public bool IsBought { get; set; }
        public bool IsSellOrderPlaced { get; set; }
        public bool IsBuyOrderPlaced { get; set; }

        public override string ToString()
        {
            return Price + " " + Amount + " " + Distance + " " + IsBought + " " + IsBuyOrderPlaced + " " + IsSellOrderPlaced;
        }
    }
}
