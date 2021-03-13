using System.IO;
using System.Net;
using Newtonsoft.Json;
using BinanceApiLibrary.Cryptocurrencies;
using System.Collections.Generic;

namespace BinanceApiLibrary.Models
{
    public static class MarketInfo
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

            Cryptocurrency cryptoInfo = JsonConvert.DeserializeObject<Cryptocurrency>(response);

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
        public static string Get24HourStatOnAsset(string symbol)
        {
            string url = $"https://api.binance.com/api/v3/ticker/24hr?symbol={symbol}";
            HttpWebRequest HTTPrequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse HTTPresponse = (HttpWebResponse)HTTPrequest.GetResponse();

            string response;

            using (StreamReader reader = new StreamReader(HTTPresponse.GetResponseStream()))
            {
                response = reader.ReadToEnd();
            }
            return response;
        }
    }
}
