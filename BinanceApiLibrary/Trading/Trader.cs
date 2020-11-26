using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace BinanceApiLibrary.Trading
{
    public static class Trader
    {
        public static string PlaceNewLimitOrder(BinanceApiUser user, string symbol, string side, string quantity, string price)
        {
            string baseUrl = "https://api.binance.com/";
            string orderUrl = "api/v3/order?";
            string url = baseUrl + orderUrl;
            string parameters = $"symbol={symbol}&side={side}&type=LIMIT&timeInForce=GTC&quantity={quantity}&price={price}&recvWindow=10000&timestamp=" + MarketInfo.GetTimestamp();
            url += parameters + "&signature=" + user.Sign(parameters);

            string response;

            HttpWebRequest HTTPrequest = (HttpWebRequest)WebRequest.Create(url);
            HTTPrequest.Headers.Add("X-MBX-APIKEY", user.ApiPublicKey);
            HTTPrequest.Method = "POST";
            HttpWebResponse HTTPresponse = (HttpWebResponse)HTTPrequest.GetResponse();

            using (StreamReader reader = new StreamReader(HTTPresponse.GetResponseStream()))
            {
                response = reader.ReadToEnd();
            }

            Console.WriteLine(response);
            return response;
        }
    }
}
