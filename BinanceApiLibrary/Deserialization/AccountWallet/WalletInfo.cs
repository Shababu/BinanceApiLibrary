using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinanceApiLibrary.Deserialization.AccountWallet
{
    public class WalletDeserialization
    {
        public Balances[] Balances { get; set; }
        
        public static List<Balances> DeserializeWalletInfo(string jsonString)
        {
            WalletDeserialization info = JsonConvert.DeserializeObject<WalletDeserialization>(jsonString);
            List<Balances> assets = info.Balances.Where(b => Convert.ToDouble(b.Free.Replace('.', ',')) > 0).ToList();
            return assets;
        }
    }

    public class Balances
    {
        public string Asset { get; set; }

        public string Free { get; set; }

        public override string ToString()
        {
            return Asset + ": " + Free;
        }
    }
}
