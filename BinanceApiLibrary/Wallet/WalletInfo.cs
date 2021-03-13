using BinanceApiLibrary.Deserialization.AccountWallet;
using BinanceApiLibrary.Models;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace BinanceApiLibrary.Wallet
{
    public static class WalletInfo
    {
        public static string BaseUrl { get => "https://api.binance.com/"; }
        public static string AccountInfoUrl { get => "api/v3/account?"; }

        public static List<AccountBalance> GetWalletInfo(BinanceApiUser user)
        {
            string response;

            string url = BaseUrl + AccountInfoUrl;
            string parameters = "recvWindow=10000&timestamp=" + MarketInfo.GetTimestamp();
            url += parameters + "&signature=" + user.Sign(parameters);

            HttpWebRequest HTTPrequest = (HttpWebRequest)WebRequest.Create(url);
            HTTPrequest.Headers.Add("X-MBX-APIKEY", user.ApiPublicKey);
            HttpWebResponse HTTPresponse = (HttpWebResponse)HTTPrequest.GetResponse();

            using (StreamReader reader = new StreamReader(HTTPresponse.GetResponseStream()))
            {
                response = reader.ReadToEnd();
            }

            return BinanceApiLibrary.Deserialization.AccountWallet.AccountWallet.DeserializeWalletInfo(response);
        }
    }
}
