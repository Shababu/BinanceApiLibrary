using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BinanceApiLibrary.Deserialization.AccountWallet
{
    public class AccountWallet
    {
        public AccountBalance[] Balances { get; set; }
        
        public static List<AccountBalance> DeserializeWalletInfo(string jsonString)
        {
            AccountWallet info = JsonConvert.DeserializeObject<AccountWallet>(jsonString);
            List<AccountBalance> assets = info.Balances.Where(b => Convert.ToDouble(b.Free.Replace('.', ',')) > 0).ToList();
            return assets;
        }
    }
}
