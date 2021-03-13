using System;
using System.Collections.Generic;
using BinanceApiLibrary.Models;
using BinanceApiLibrary.Deserialization.AccountWallet;
using BinanceApiLibrary.Wallet;
using BinanceApiLibrary.Deserialization.Trades;
using BinanceApiLibrary.TradeHistory;
using BinanceApiLibrary.Cryptocurrencies;
using BinanceApiLibrary.Trading;
using System.Threading;

namespace BinanceAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            // Пример использования: Вывод баланса всех кошельков, которые не пустые
            // BinanceApiUser user = new BinanceApiUser("публичный ключ", "секретный ключ");
            // List<AccountBalance> wallet = WalletInfo.GetWalletInfo(user);
            // foreach (var asset in wallet)
            // {
            //     Console.WriteLine(asset);
            // }


            // Пример использования: Вывод торговых операций по торговой паре от более новых к более старым
            // BinanceApiUser user = new BinanceApiUser("публичный ключ", "секретный ключ");
            // List<FilledTrade> trades = AccountInfo.GetTrades(user, "XRPBUSD");
            // foreach (var trade in trades)
            // {
            //     Console.WriteLine(trade.ToString() + "\n");
            // }


            // Пример использования: Размещение ордера
            // BinanceApiUser user = new BinanceApiUser("публичный ключ", "секретный ключ");
            // Trader.PlaceNewLimitOrder(user, "XRPUSDT", "SELL", "50", "0.70");


            // Пример использования: Статистика о торговой апре за последние 24 часа
            // AssetStats stats = AssetStats.DeserializeAssetStats(MarketInfo.Get24HourStatOnAsset("XRPUPUSDT"));
            // Console.WriteLine(stats.ToString());
            // Console.ReadLine();


            BinanceApiUser user = new BinanceApiUser("Публичный ключ", "Приватный ключ");
            Cryptocurrency coinToTrade = new Cryptocurrency("XRPBUSD", "XRP");
            string configPath = @"C:/Users/Саид/Desktop/TradeConfig.txt";

            List<SpotPosition> orders = Trader.ReadTradeStateFromFile(configPath);

            Trader.CheckOrders(user, orders, coinToTrade, configPath);

            while (true)
            {
                Trader.Analize(user, orders, coinToTrade, configPath);
                if (DateTime.Now.Second == 0)
                {
                    Console.Clear();
                }
                Thread.Sleep(200);
            }
        }
    }
}