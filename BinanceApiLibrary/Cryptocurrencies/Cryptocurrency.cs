using System;
using System.Collections.Generic;
using System.Text;

namespace BinanceApiLibrary.Cryptocurrencies
{
    public class Cryptocurrency
    {
        public string Symbol { get; set; }
        public float Price { get; set; }

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

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
