using System;
using System.Collections.Generic;
using BinanceApiLibrary.Cryptocurrencies;
using BinanceApiLibrary.Deserialization;
using BinanceApiLibrary.Models;
using BinanceApiLibrary.Trading;
using Xunit;

namespace BinanceApiUnitTests
{
    public class TraderTest
    {
        [Fact]
        public void Trader_CheckOrders()
        {
            BinanceApiUser user = new BinanceApiUser("Публичный ключ", "Приватный ключ");
            string configPath = @"C:/Users/Саид/Desktop/TradeConfig.txt";
            List<Position> orders = Trader.ReadTradeStateFromFile(configPath);
            Cryptocurrency cryptocurrency = new Cryptocurrency("XRPBUSD", "XRP");

            Trader.CheckOrders(user, orders, cryptocurrency, configPath);

            foreach(var order in orders)
            {
                Assert.True(order.IsBought);
                Assert.True(order.IsBuyOrderPlaced);
                Assert.True(order.IsSellOrderPlaced);
            }
        }
    }
}
