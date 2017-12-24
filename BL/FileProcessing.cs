using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.Models;
using System.Collections.ObjectModel;
using CoinMarketApi;

namespace BL
{
    public class FileProcessing : IBL
    {
        ICurrencies currencies = new Currencies();

        public ObservableCollection<CurrencyModel> GetCurrency()
        {
            return currencies.GetCurrency();
        }
    }
}
