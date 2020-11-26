using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinanceApiLibrary.Deserialization
{
    public class Trade
    {
        public string Symbol { get; set; }
        public double Price { get; set; }
        public double Qty { get; set; }
        public double QuoteQty { get; set; }
        public double Commission { get; set; }
        public string TimeStamp { get; set; }
        public bool IsBuyer { get; set; }
        public bool IsMaker { get; set; }

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
            return string.Format($"{side}, Symbol: {Symbol}, Price: {Price}, Quantuty {boughtOrSold}: {Qty}, Quantuty {spentOrRecived}: {QuoteQty}, Commission: {Commission}");
        }
    }     
}
