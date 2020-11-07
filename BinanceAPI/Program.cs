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

namespace BinanceAPI
{
    class Program
    {
        static void Main(string[] args)
        {

            //List<Balances> wallet = MarketInfo.GetWalletInfo(user);
                
            //foreach(var asset in wallet)
            //{
            //    Console.WriteLine(asset.Asset + ": " + asset.Free);
            //}

        }
    }
}
