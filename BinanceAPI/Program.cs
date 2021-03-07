using System;
using System.Collections.Generic;
using BinanceApiLibrary.Cryptocurrencies;
using System.Threading;
using BinanceApiLibrary.Trading;
using BinanceApiLibrary.Models;

namespace BinanceAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            // Пример использования: Вывод баланса всех кошельков, которые не пустые
            // BinanceApiUser user = new BinanceApiUser("публичный ключ", "секретный ключ");
            // List<Balances> wallet = MarketInfo.GetWalletInfo(user);
            // foreach (var asset in wallet)
            // {
            //     Console.WriteLine(asset);
            // }


            // Пример использования: Вывод торговых операций по торговой паре от более новых к более старым
            // BinanceApiUser user = new BinanceApiUser("публичный ключ", "секретный ключ");
            // List<Trade> trades = AccountInfo.GetTrades(user, "ONEUSDT");
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

            BinanceApiUser user = new BinanceApiUser("Публичнй ключ", "Приватный ключ"); // СЮДА НАДО ВСТАВИТЬ СВОИ КЛЮЧИ!!!
            Cryptocurrency coinToTrade = new Cryptocurrency("XRPBUSD", "XRP");
            string configPath = @"C:/Users/Саид/Desktop/TradeConfig.txt";

            List<Position> orders = Trader.ReadTradeStateFromFile(configPath);

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
