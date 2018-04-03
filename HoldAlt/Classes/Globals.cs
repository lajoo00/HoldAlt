using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net;
using System.Threading;
using HoldAlt.Models;

namespace HoldAlt.Classes
{
    public class Globals
    {
        public static string[] strCoinName = new string[100];
        public static string[] strCoinSymbol = new string[100];
        public static string[] strAPICode = new string[100];
        public static int intNumAltCoins;
        public static string[] strExchanges = new string[100];
        public static int intNumExch;
        public static List<AltCoin> lAltCoins = new List<AltCoin>();

        public static string BTCtoUSDtoday()
        {
            // Value today
            string price = "";

            var strValue = string.Empty;
            var strRequest = (HttpWebRequest)WebRequest.Create("https://api.coinmarketcap.com/v1/ticker/bitcoin/");
            strRequest.Timeout = 5000;
            strRequest.Method = "GET";
            strRequest.ContentType = "text/plain";
            using (var _webResponse = (HttpWebResponse)strRequest.GetResponse())
            {
                var _webResponseStatus = _webResponse.StatusCode;
                var _stream = _webResponse.GetResponseStream();
                using (var _streamReader = new StreamReader(_stream))
                {
                    strValue = _streamReader.ReadToEnd();
                }
            }

            if (strValue != string.Empty)
            {
                int index = strValue.IndexOf("price_usd") + 13;
                if (index > -1)
                {
                    int index2;
                    // Get coin name
                    index2 = strValue.IndexOf("\"", index);
                    price = strValue.Substring(index, index2 - index);
                    //MessageBox.Show(strCoinName[coinIndex-1]);
                }
            }

            return price;
        }

        public static string CoinToUSDtoday(string coin)
        {
            // Value today
            string price = "";

            var strValue = string.Empty;
            var strRequest = (HttpWebRequest)WebRequest.Create("https://api.coinmarketcap.com/v1/ticker/" + coin + "/");
            strRequest.Timeout = 5000;
            strRequest.Method = "GET";
            strRequest.ContentType = "text/plain";
            using (var _webResponse = (HttpWebResponse)strRequest.GetResponse())
            {
                var _webResponseStatus = _webResponse.StatusCode;
                var _stream = _webResponse.GetResponseStream();
                using (var _streamReader = new StreamReader(_stream))
                {
                    strValue = _streamReader.ReadToEnd();
                }
            }

            if (strValue != string.Empty)
            {
                int index = strValue.IndexOf("price_usd") + 13;
                if (index > -1)
                {
                    int index2;
                    // Get coin name
                    index2 = strValue.IndexOf("\"", index);
                    price = strValue.Substring(index, index2 - index);
                    //MessageBox.Show(strCoinName[coinIndex-1]);
                }
            }

            return price;
        }

        public static float PctChange(string coin, int days)
        {
            // Value today
            string sRet = string.Empty;

            var strValue = string.Empty;
            var strRequest = (HttpWebRequest)WebRequest.Create("https://api.coinmarketcap.com/v1/ticker/" + coin + "/");
            strRequest.Timeout = 5000;
            strRequest.Method = "GET";
            strRequest.ContentType = "text/plain";
            using (var _webResponse = (HttpWebResponse)strRequest.GetResponse())
            {
                var _webResponseStatus = _webResponse.StatusCode;
                var _stream = _webResponse.GetResponseStream();
                using (var _streamReader = new StreamReader(_stream))
                {
                    strValue = _streamReader.ReadToEnd();
                }
            }

            if (strValue != string.Empty)
            {
                int index;
                if (days == 7)
                    index = strValue.IndexOf("percent_change_7d") + 21;
                else
                    index = strValue.IndexOf("percent_change_24h") + 22;

                if (index > -1)
                {
                    int index2;
                    // Get coin name
                    index2 = strValue.IndexOf("\"", index);
                    sRet = strValue.Substring(index, index2 - index);
                    //MessageBox.Show(strCoinName[coinIndex-1]);
                }
            }

            return Convert.ToSingle(sRet);
        }

        public static void MyDoEvents()
        {
            // WPF replacement for DoEvents
            //Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate { }));

        }

        public static bool IsNumeric(object Expression)
        {
            double retNum;

            bool isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }
    }
}