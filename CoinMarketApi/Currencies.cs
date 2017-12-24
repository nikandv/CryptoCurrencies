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
                Logger.Log.Info("Начало загрузки данных с coinmarketcap.com");
                Uri uri = new Uri($"https://api.coinmarketcap.com/v1/ticker/?convert={CONVERT}&limit={LIMIT}", UriKind.Absolute);
                using (WebClient wc = new WebClient())
                {
                    json = wc.DownloadString(uri);
                }
                Logger.Log.Info("Загрузка данных произведена успешно");
            }
            catch(WebException ex)
            {
                Logger.Log.Error("Отстствует подключение к интернету");
            }
            catch(Exception ex)
            {
                Logger.Log.Error($"Во время загрузки данных произошла ошибка: {ex.Message}");
            }
            if(string.IsNullOrWhiteSpace(json))
            {
                Logger.Log.Info("В результате загрузки получены пустые данные");
                throw new ArgumentNullException("Данные не получены");
            }
            ObservableCollection<CurrenciesRootObject> root = new ObservableCollection<CurrenciesRootObject>();
            try
            {
                Logger.Log.Info("Начало десериализации данных с сайта");
                root = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<CurrenciesRootObject>>(json);
                Logger.Log.Info("Десериализация данных произведена успешно");
            }
            catch(Exception ex)
            {
                Logger.Log.Error($"Произошла ошибка во время десериализации данных: {ex.Message}");
            }
            if (root[0] == null)
            {
                Logger.Log.Error("Десериализованные данные имеют неправильный формат");
                throw new ArgumentException("Полученные данные имели недопустимый формат");
            }
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
            Logger.Log.Info("Получение данных произведено успешно");
            return currencies;
        }
    }
}
