using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using BinanceApiLibrary;
using BinanceApiLibrary.Cryptocurrencies;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;
using Newtonsoft.Json;
using BinanceApiLibrary.Deserialization.AccountWallet;
using BinanceApiLibrary.Deserialization;
using BinanceApiLibrary.Trading;

namespace BinanceAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            // Пример использования: Вывод баланса всех кошельков, которые не пустые
            // BinanceApiUser user = new BinanceApiUser("публичный ключ", "секретный ключ");
            //List<Balances> wallet = MarketInfo.GetWalletInfo(user);
            //foreach (var asset in wallet)
            //{
            //    Console.WriteLine(asset);
            //}


            // Пример использования: Вывод торговых операций по торговой паре от более новых к более старым
            // BinanceApiUser user = new BinanceApiUser("публичный ключ", "секретный ключ");
            //List<Trade> trades = AccountInfo.GetTrades(user, "ONEUSDT");
            //foreach (var trade in trades)
            //{
            //    Console.WriteLine(trade.ToString() + "\n");
            //}


            // Пример использования: Размещение ордера
            // BinanceApiUser user = new BinanceApiUser("публичный ключ", "секретный ключ");
            // Trader.PlaceNewLimitOrder(user, "XRPUSDT", "SELL", "50", "0.70");


            Console.ReadLine();                
        }
    }
}
