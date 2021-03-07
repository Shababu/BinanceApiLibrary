using BinanceApiLibrary.Deserialization;
using BinanceApiLibrary.Deserialization.AccountWallet;
using BinanceApiLibrary.Deserialization.Trades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Linq;
using BinanceApiLibrary.Cryptocurrencies;
using BinanceApiLibrary.Models;

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
            return response;
        }

        public static void Analize(BinanceApiUser user, List<Position> positions, Cryptocurrency crypto, string configPath)
        {
            try
            {
                decimal priceOfAsset = Convert.ToDecimal(AssetStats.DeserializeAssetStats(MarketInfo.Get24HourStatOnAsset(crypto.Symbol)).LastPrice.Replace('.', ','));

                foreach (var position in positions)
                {
                    if (IsOrderCheckNeeded(position, priceOfAsset))
                    {
                        if (!position.IsBought && !position.IsBuyOrderPlaced && CanTrade(user))
                        {
                            PlaceNewLimitOrder(user, crypto.Symbol, "BUY", position.Amount.ToString().Replace(',','.'), position.Price.ToString().Replace(',', '.'));
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"Ордер на покупку {crypto.Name} Выставлен");
                            Console.ForegroundColor = ConsoleColor.White;
                            position.IsBuyOrderPlaced = true;
                            WriteTradeStateToFile(configPath, position);
                        }

                        else if (!position.IsBought && position.IsBuyOrderPlaced)
                        {
                            if (priceOfAsset < position.Price)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"Покупка {crypto.Name}");
                                Console.ForegroundColor = ConsoleColor.White;

                                position.IsBought = true;
                                WriteTradeStateToFile(configPath, position);

                                PlaceNewLimitOrder(user, crypto.Symbol, "SELL", position.Amount.ToString().Replace(',', '.'), (position.Price + position.Distance).ToString().Replace(',', '.'));
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"Ордер на продажу {crypto.Name} Выставлен");
                                Console.ForegroundColor = ConsoleColor.White;
                                position.IsSellOrderPlaced = true;
                                WriteTradeStateToFile(configPath, position);
                            }
                        }

                        else if (position.IsBought && !position.IsSellOrderPlaced && CanTrade(user))
                        {
                            PlaceNewLimitOrder(user, crypto.Symbol, "SELL", position.Amount.ToString().Replace(',', '.'), (position.Price + position.Distance).ToString().Replace(',', '.'));
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Ордер на продажу {crypto.Name} Выставлен");
                            Console.ForegroundColor = ConsoleColor.White;
                            position.IsSellOrderPlaced = true;
                            WriteTradeStateToFile(configPath, position);

                        }

                        if (position.IsBought && position.IsSellOrderPlaced && priceOfAsset > (position.Price + position.Distance))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"Продажа {crypto.Name}");
                            Console.ForegroundColor = ConsoleColor.White;

                            IncreaseAmount(position);

                            position.IsBought = position.IsBought = position.IsSellOrderPlaced = false;
                            WriteTradeStateToFile(configPath, position);

                            PlaceNewLimitOrder(user, crypto.Symbol, "BUY", position.Amount.ToString().Replace(',', '.'), position.Price.ToString().Replace(',', '.'));
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"Ордер на покупку {crypto.Name} Выставлен");
                            Console.ForegroundColor = ConsoleColor.White;
                            position.IsBuyOrderPlaced = true;

                            WriteTradeStateToFile(configPath, position);
                        }

                    }
                }
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(priceOfAsset.ToString().Substring(0, 6) + " ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(DateTime.Now.Second + " Проход по циклу завершен");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибочка" + ex.Message);
            }
        }

        public static bool CanTrade(BinanceApiUser user)
        {
            return true;
        }

        public static void CheckOrders(BinanceApiUser user, List<Position> positions, Cryptocurrency crypto, string configPath)
        {
            decimal priceOfAsset = Convert.ToDecimal(AssetStats.DeserializeAssetStats(MarketInfo.Get24HourStatOnAsset(crypto.Symbol)).LastPrice.Replace('.', ','));

            foreach (var position in positions)
            {
                if ( (!position.IsBought && !position.IsBuyOrderPlaced) && priceOfAsset < position.Price)
                {
                    PlaceNewLimitOrder(user, crypto.Symbol, "BUY", position.Amount.ToString().Replace(',', '.'), position.Price.ToString().Replace(',', '.'));
                    position.IsBuyOrderPlaced = position.IsBought = true;
                    WriteTradeStateToFile(configPath, position);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Покупка {crypto.Name}");
                    Console.ForegroundColor = ConsoleColor.White;

                    PlaceNewLimitOrder(user, crypto.Symbol, "SELL", position.Amount.ToString().Replace(',', '.'), (position.Price + position.Distance * 0.96M).ToString().Replace(',', '.'));
                    position.IsSellOrderPlaced = true;
                    WriteTradeStateToFile(configPath, position);
                }

                if ( (position.IsBought && !position.IsSellOrderPlaced) && priceOfAsset > position.Price)
                {
                    PlaceNewLimitOrder(user, crypto.Symbol, "SELL", position.Amount.ToString().Replace(',', '.'), (position.Price + position.Distance * 0.96M).ToString().Replace(',', '.'));
                    position.IsBought = position.IsSellOrderPlaced = position.IsBuyOrderPlaced = false;
                    WriteTradeStateToFile(configPath, position);

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Продажа {crypto.Name}");
                    Console.ForegroundColor = ConsoleColor.White;

                    PlaceNewLimitOrder(user, crypto.Symbol, "BUY", position.Amount.ToString().Replace(',', '.'), position.Price.ToString().Replace(',', '.'));
                    position.IsBuyOrderPlaced = true;
                    WriteTradeStateToFile(configPath, position);
                }

                if (!position.IsSellOrderPlaced && position.IsBought)
                {
                    PlaceNewLimitOrder(user, crypto.Symbol, "SELL", position.Amount.ToString().Replace(',', '.'), (position.Price + position.Distance).ToString().Replace(',', '.'));
                    position.IsSellOrderPlaced = true;

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Ордер на продажу {crypto.Name} выставлен");
                    Console.ForegroundColor = ConsoleColor.White;

                    WriteTradeStateToFile(configPath, position);
                }

                if (!position.IsBuyOrderPlaced)
                {
                    PlaceNewLimitOrder(user, crypto.Symbol, "BUY", position.Amount.ToString().Replace(',', '.'), position.Price.ToString().Replace(',', '.'));
                    position.IsBuyOrderPlaced = true;

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Ордер на покупку {crypto.Name} выставлен");
                    Console.ForegroundColor = ConsoleColor.White;

                    WriteTradeStateToFile(configPath, position);
                }
            }
        }

        public static List<Position> ReadTradeStateFromFile(string path)
        {
            List<Position> positions = new List<Position>();

            using (StreamReader stream = new StreamReader(path))
            {
                string orderRawInfo = stream.ReadLine();
                while (orderRawInfo != null)
                {
                    string[] positionInfoArray = orderRawInfo.Split(' ');
                    Position position = new Position();
                    position.Price = Convert.ToDecimal(positionInfoArray[0]);
                    position.Amount = Convert.ToDecimal(positionInfoArray[1]);
                    position.Distance = Convert.ToDecimal(positionInfoArray[2]);
                    position.IsBought = Convert.ToBoolean(positionInfoArray[3]);
                    position.IsBuyOrderPlaced = Convert.ToBoolean(positionInfoArray[4]);
                    position.IsSellOrderPlaced = Convert.ToBoolean(positionInfoArray[5]);

                    positions.Add(position);

                    orderRawInfo = stream.ReadLine();
                }
            }

            return positions;
        }

        public static void WriteTradeStateToFile(string path, Position position)
        {
            List<Position> positions = ReadTradeStateFromFile(path);

            using (StreamWriter stream = new StreamWriter(path))
            {
                for (var i = 0; i < positions.Count; i++)
                {
                    if (positions[i].Price == position.Price)
                    {
                        positions[i] = position;
                        break;
                    }
                }

                foreach (var item in positions)
                {
                    stream.WriteLine(item.ToString());
                }
            }
        }

        public static void IncreaseAmount(Position position)
        {
            decimal newDollarAmount = (((position.Amount * (position.Price + position.Distance)) - (position.Price * position.Amount)) * .99M) + (position.Price * position.Amount); // 13,87

            position.Amount = newDollarAmount / position.Price;
            string middleValueInt = position.Amount.ToString();
            string middleValueDecimal = middleValueInt.Split(',')[1].Substring(0, 1);
            middleValueInt = middleValueInt.Split(',')[0];
            position.Amount = Convert.ToDecimal((middleValueInt + "," + middleValueDecimal));
        }




        public static bool IsOrderCheckNeeded(Position position, decimal priceOfAsset)
        {
            if (priceOfAsset >= (position.Price - position.Price / 60) &&
               (priceOfAsset < position.Price + position.Price / 60) ||
                priceOfAsset > (position.Price + position.Distance - position.Price / 60) &&
                priceOfAsset < (position.Price + position.Distance + position.Price / 60)
            )
            {
                return true;
            }
            return false;
        }

        public static string PlaceNewLimitOrder2(BinanceApiUser user, string symbol, string side, string quantity, string price)
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
            return Order.DeserializeOrder(response).OrderId;
        }
    }
}
