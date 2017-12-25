using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.Models;
using System.Collections.ObjectModel;
using CoinMarketApi;
using System.IO;
using DAL;

namespace BL
{
    public class FileProcessing : IBL
    {
        ICurrencies currencies = new Currencies();
        FileReader fileReader;
        Config config;

        public FileProcessing(Config config)
        {
            Logger.InitLogger();
            this.config = config;
            fileReader = new FileReader();
        }

        public ObservableCollection<CurrencyModel> GetCurrency()
        {
            return currencies.GetCurrency();
        }

        public ObservableCollection<ShortNames> GetShortNames()
        {
            return fileReader.GetData(config.NamesPath);
        }

        public void SaveData()
        {
            fileReader.SaveData(GetCurrency(), config.DataPath);
        }

        public void SaveAsWord(string text)
        {
            fileReader.SaveAsWord(text);
        }
    }
}
