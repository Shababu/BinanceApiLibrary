namespace BinanceApiLibrary.Cryptocurrencies
{
    public class Cryptocurrency
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        public float Price { get; set; }

        public Cryptocurrency(string symbol, string name)
        {
            Symbol = symbol;
            Name = name;
        }
        public Cryptocurrency(string symbol, float price)
        {
            Symbol = symbol;
            Price = price;
        }
        public Cryptocurrency()
        {
            Symbol = "XRPUSDT";
        }

        public override string ToString()
        {
            return Symbol;
        }
    }
}
