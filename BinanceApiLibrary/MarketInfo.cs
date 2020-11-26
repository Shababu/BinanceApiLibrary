using System;
using System.IO;
using System.Net;
using Newtonsoft;
using Newtonsoft.Json;
using BinanceApiLibrary.Cryptocurrencies;
using System.Collections.Generic;
using System.Transactions;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using BinanceApiLibrary.Deserialization.AccountWallet;

namespace BinanceApiLibrary
{
    public class MarketInfo
    {
        public static string BaseUrl { get => "https://api.binance.com/"; }
        public static string AccountInfoUrl { get => "api/v3/account?"; }
        public static Cryptocurrency GetPrice(string pairSymbol = "XRPUSDT")
        {
            string url = $"https://api.binance.com/api/v3/ticker/price?symbol={pairSymbol}";

            HttpWebRequest HTTPrequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse HTTPresponse = (HttpWebResponse)HTTPrequest.GetResponse();

            string response;

            using (StreamReader reader = new StreamReader(HTTPresponse.GetResponseStream()))
            {
                response = reader.ReadToEnd();
            }

            Cryptocurrency cryptoInfo = JsonConvert.DeserializeObject<Xrp>(response);

            return cryptoInfo;
        }

        public static List<Cryptocurrency> GetPrice(params string[] pairSymbols)
        {
            List<Cryptocurrency> traidingPairs = new List<Cryptocurrency>();
            for(int i = 0; i < pairSymbols.Length; i++)
            {
                traidingPairs.Add(new Cryptocurrency(pairSymbols[i], (GetPrice(pairSymbols[i])).Price));
            }

            return traidingPairs;
        }

        public static string GetTimestamp()
        {
            string url = $"https://api.binance.com/api/v3/time";
            HttpWebRequest HTTPrequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse HTTPresponse = (HttpWebResponse)HTTPrequest.GetResponse();

            string response;

            using (StreamReader reader = new StreamReader(HTTPresponse.GetResponseStream()))
            {
                response = reader.ReadToEnd();
            }
            response = response.Substring(14).Trim('}');
            return response;
        }

        public static List<Balances> GetWalletInfo(BinanceApiUser user)
        {
            string response;

            string url = BaseUrl + AccountInfoUrl;
            string parameters = "recvWindow=10000&timestamp=" + GetTimestamp();
            url += parameters + "&signature=" + user.Sign(parameters);

            HttpWebRequest HTTPrequest = (HttpWebRequest)WebRequest.Create(url);
            HTTPrequest.Headers.Add("X-MBX-APIKEY", user.ApiPublicKey);
            HttpWebResponse HTTPresponse = (HttpWebResponse)HTTPrequest.GetResponse();

            using (StreamReader reader = new StreamReader(HTTPresponse.GetResponseStream()))
            {
                response = reader.ReadToEnd();
            }

            return WalletDeserialization.DeserializeWalletInfo(response);
        }
    }
}
