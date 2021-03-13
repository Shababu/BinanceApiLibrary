using BinanceApiLibrary.Deserialization.Trades;
using BinanceApiLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace BinanceApiLibrary.TradeHistory
{
    public static class AccountInfo
    {
        public static string BaseUrl { get => "https://api.binance.com/"; }
        public static string TradeUrl { get => "api/v3/myTrades?"; }
        public static List<FilledTrade> GetTrades(BinanceApiUser user, string symbol)
        {
            string response;

            string url = BaseUrl + TradeUrl;
            string parameters = "recvWindow=10000&symbol=" + symbol + "&timestamp=" + MarketInfo.GetTimestamp();
            url += parameters + "&signature=" + user.Sign(parameters);

            HttpWebRequest HTTPrequest = (HttpWebRequest)WebRequest.Create(url);
            HTTPrequest.Headers.Add("X-MBX-APIKEY", user.ApiPublicKey);
            HttpWebResponse HTTPresponse = (HttpWebResponse)HTTPrequest.GetResponse();

            using (StreamReader reader = new StreamReader(HTTPresponse.GetResponseStream()))
            {
                response = reader.ReadToEnd();
            }

            List<object> trades = JsonConvert.DeserializeObject<List<object>>(response);
            List<FilledTrade> listOfTrades = new List<FilledTrade>();
            foreach (var trade in trades)
            {
                FilledTrade tradeInfo = new FilledTrade();
                string tradeString = trade.ToString();
                tradeString = tradeString.Trim('{', '}');
                string[] tradeStrings = tradeString.Split(',');
                tradeInfo.OrderId = tradeStrings[1].Substring(10);
                tradeInfo.Symbol = tradeStrings[0].Substring(15).Trim('"');
                tradeInfo.Price = Convert.ToDouble(tradeStrings[4].Substring(13).Trim('"').Replace('.', ','));
                tradeInfo.Qty = Convert.ToDouble(tradeStrings[5].Substring(11).Trim('"').Replace('.', ','));
                tradeInfo.QuoteQty = Convert.ToDouble(tradeStrings[6].Substring(16).Trim('"').Replace('.', ','));
                tradeInfo.Commission = Convert.ToDouble(tradeStrings[7].Substring(18).Trim('"').Replace('.', ','));
                tradeInfo.TimeStamp = tradeStrings[9].Substring(12).Trim('"');
                tradeInfo.IsBuyer = Convert.ToBoolean(tradeStrings[10].Substring(15).Trim('"'));
                tradeInfo.IsMaker = Convert.ToBoolean(tradeStrings[11].Substring(15).Trim('"'));
                listOfTrades.Add(tradeInfo);
            }
            listOfTrades.Reverse();
            return listOfTrades;  
        }
    }
}
