using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.Models;
using System.Net;
using System.Collections.ObjectModel;

namespace CoinMarketApi
{
    public class Currencies : ICurrencies
    {
        private const int LIMIT = 30;
        private const string CONVERT = "RUB";

        public ObservableCollection<CurrencyModel> GetCurrency()
        {
            string json = string.Empty;
            try
            {
                Uri uri = new Uri($"https://api.coinmarketcap.com/v1/ticker/?convert={CONVERT}&limit={LIMIT}", UriKind.Absolute);
                using (WebClient wc = new WebClient())
                {
                    json = wc.DownloadString(uri);
                }
            }
            catch(WebException ex)
            {

            }
            if(string.IsNullOrWhiteSpace(json))
            {
                throw new ArgumentNullException("Данные не получены");
            }
            ObservableCollection<CurrenciesRootObject> root = new ObservableCollection<CurrenciesRootObject>();
            try
            {
                root = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<CurrenciesRootObject>>(json);
            }
            catch(Exception ex)
            {

            }
            if (root[0] == null)
                throw new ArgumentException("Полученные данные имели недопустимый формат");
            ObservableCollection<CurrencyModel> currencies = new ObservableCollection<CurrencyModel>();
            foreach (var item in root)
            {
                currencies.Add(new CurrencyModel()
                {
                    Name = item.name,
                    Rank = item.rank,
                    Difference = string.Format("{0:0.##}", double.Parse(item.percent_change_24h, System.Globalization.CultureInfo.InvariantCulture)) + "%",
                    ValueInRUB = Math.Round(double.Parse(item.price_rub, System.Globalization.CultureInfo.InvariantCulture)) + "₽",
                    ValueInUSD = "$" + string.Format("{0:0.##}", double.Parse(item.price_usd, System.Globalization.CultureInfo.InvariantCulture)),
                });
            }
            return currencies;
        }
    }
}
